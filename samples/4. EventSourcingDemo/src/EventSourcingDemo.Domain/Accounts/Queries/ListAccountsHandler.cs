using Brickweave.Cqrs;
using EventSourcingDemo.Domain.Accounts.Models;
using EventSourcingDemo.Domain.Accounts.Services;

namespace EventSourcingDemo.Domain.Accounts.Queries
{
    public class ListAccountsHandler : IQueryHandler<ListAccounts, IEnumerable<AccountInfo>>
    {
        private readonly IAccountRepository _accountRepository;

        public ListAccountsHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IEnumerable<AccountInfo>> HandleAsync(ListAccounts query)
        {
            var results = await _accountRepository.ListAccountsAsync();

            return results
                .OrderBy(p => p.Name)
                .ToList();
        }
    }
}
