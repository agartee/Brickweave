using Brickweave.Cqrs;
using EventSourcingDemo.Domain.Accounts.Extensions;
using EventSourcingDemo.Domain.Accounts.Models;
using EventSourcingDemo.Domain.Accounts.Services;
using EventSourcingDemo.Domain.People.Models;
using EventSourcingDemo.Domain.People.Services;

namespace EventSourcingDemo.Domain.Accounts.Commands
{
    public class CreatePersonalAccountHandler : ICommandHandler<CreatePersonalAccount, AccountInfo>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPersonRepository _personRepository;

        public CreatePersonalAccountHandler(IAccountRepository accountRepository, IPersonRepository personRepository)
        {
            _accountRepository = accountRepository;
            _personRepository = personRepository;
        }

        public async Task<AccountInfo> HandleAsync(CreatePersonalAccount command)
        {
            var account = new Account(AccountId.NewId(), command.Name, command.AccountHolderId);

            await _accountRepository.SaveAccountAsync(account);

            var accountHolder = await _personRepository.DemandPersonInfoAsync((PersonId) account.AccountHolderId);

            return account.ToAccountInfo(accountHolder);
        }
    }
}
