using Brickweave.Cqrs;
using EventSourcingDemo.Domain.Accounts.Models;
using EventSourcingDemo.Domain.Common.Models;
using EventSourcingDemo.Domain.People.Models;

namespace EventSourcingDemo.Domain.Accounts.Commands
{
    public class CreatePersonalAccount : ICommand<AccountInfo>
    {
        public CreatePersonalAccount(Name name, PersonId accountHolderId)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            AccountHolderId = accountHolderId ?? throw new ArgumentNullException(nameof(accountHolderId));
        }

        public Name Name { get; }
        public PersonId AccountHolderId { get; }
    }
}
