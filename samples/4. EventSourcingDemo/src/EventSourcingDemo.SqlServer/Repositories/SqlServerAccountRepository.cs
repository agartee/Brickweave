using Brickweave.EventStore.Factories;
using Brickweave.EventStore.SqlServer;
using Brickweave.Messaging.SqlServer.Extensions;
using Brickweave.Serialization;
using EventSourcingDemo.Domain.Accounts.Models;
using EventSourcingDemo.Domain.Accounts.Services;
using EventSourcingDemo.Domain.Common.Exceptions;
using EventSourcingDemo.Domain.People.Models;
using EventSourcingDemo.SqlServer.Entities;
using EventSourcingDemo.SqlServer.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EventSourcingDemo.SqlServer.Repositories
{
    public class SqlServerAccountRepository : AggregateRepository<Account>, IAccountRepository
    {
        private EventSourcingDemoDbContext _dbContext;
        private readonly IDocumentSerializer _serializer;

        public SqlServerAccountRepository(EventSourcingDemoDbContext dbContext, IDocumentSerializer serializer,
            IAggregateFactory aggregateFactory)
            : base(serializer, aggregateFactory)
        {
            _dbContext = dbContext;
            _serializer = serializer;
        }

        public async Task<Account> DemandAccountAsync(AccountId id)
        {
            var company = await CreateFromEventsAsync(_dbContext.Events, id.Value);

            if (company == null)
                throw new EntityNotFoundException(id, nameof(Account));

            return company;
        }

        public async Task SaveAccountAsync(Account account)
        {
            AddUncommittedEvents(_dbContext.Events, account, account.Id.Value);

            account.GetDomainEvents()
                .Enqueue(_dbContext.MessageOutbox, _serializer);

            await HandlePersonalAccountProjection(account);
            await HandleBusinessAccountProjection(account);

            await _dbContext.SaveChangesAsync();

            account.ClearUncommittedEvents();
            account.ClearDomainEvents();
        }

        public async Task<IEnumerable<AccountInfo>> ListAccountsAsync()
        {
            var data = await _dbContext.PersonalAccounts
                .Include(a => a.AccountHolder)
                .ToListAsync();

            return data.Select(a => a.ToAccountInfo());
        }

        private async Task HandlePersonalAccountProjection(Account account)
        {
            var data = await _dbContext.PersonalAccounts
                .FirstOrDefaultAsync(a => a.Id == account.Id.Value);

            if (data == null && account.IsPersonalAccount())
                CreatePersonalAccountProjection(account);
            else if (data != null && (!account.IsPersonalAccount() || !account.IsActive))
                DeletePersonalAccountProjection(data);
            else if(data != null)
                UpdatePersonalAccountProjection(data, account);
        }

        private void DeletePersonalAccountProjection(PersonalAccountData existingData)
        {
            _dbContext.PersonalAccounts.Remove(existingData);
        }

        private void CreatePersonalAccountProjection(Account account)
        {
            _dbContext.PersonalAccounts.Add(new PersonalAccountData
            {
               Id = account.Id.Value,
               Name = account.Name.Value,
               AccountHolderId = account.AccountHolderId.Value,
               Balance = account.Balance
            });
        }

        private void UpdatePersonalAccountProjection(PersonalAccountData existingData, Account account)
        {
            existingData.Name = account.Name.Value;
            existingData.AccountHolderId = account.AccountHolderId.Value;
            existingData.Balance = account.Balance;
        }

        private async Task HandleBusinessAccountProjection(Account account)
        {
            var data = await _dbContext.BusinessAccounts
                .FirstOrDefaultAsync(a => a.Id == account.Id.Value);

            if (data == null && account.IsBusinessAccount())
                CreateBusinessAccountProjection(account);
            else if (data != null && (!account.IsBusinessAccount() || !account.IsActive))
                DeleteBusinessAccountProjection(data);
            else if (data != null)
                UpdateBusinessAccountProjection(data, account);
        }

        private void DeleteBusinessAccountProjection(BusinessAccountData existingData)
        {
            _dbContext.BusinessAccounts.Remove(existingData);
        }

        private void CreateBusinessAccountProjection(Account account)
        {
            _dbContext.BusinessAccounts.Add(new BusinessAccountData
            {
                Id = account.Id.Value,
                Name = account.Name.Value,
                AccountHolderId = account.AccountHolderId.Value,
                Balance = account.Balance
            });
        }

        private void UpdateBusinessAccountProjection(BusinessAccountData existingData, Account account)
        {
            existingData.Name = account.Name.Value;
            existingData.AccountHolderId = account.AccountHolderId.Value;
            existingData.Balance = account.Balance;
        }
    }
}
