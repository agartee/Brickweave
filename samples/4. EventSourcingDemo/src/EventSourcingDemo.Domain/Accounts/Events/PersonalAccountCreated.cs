using Brickweave.Domain;
using Brickweave.EventStore;
using EventSourcingDemo.Domain.Accounts.Models;
using EventSourcingDemo.Domain.Common.Models;
using EventSourcingDemo.Domain.People.Models;

namespace EventSourcingDemo.Domain.Accounts.Events
{
    public class PersonalAccountCreated : IEvent, IDomainEvent
    {
        public PersonalAccountCreated(AccountId accountId, Name accountName, PersonId accountHolderId)
        {
            AccountId = accountId ?? throw new ArgumentNullException(nameof(accountId));
            AccountName = accountName ?? throw new ArgumentNullException(nameof(accountName));
            AccountHolderId = accountHolderId ?? throw new ArgumentNullException(nameof(accountHolderId));
        }

        public AccountId AccountId { get; }
        public Name AccountName { get; }
        public PersonId AccountHolderId { get; }
    }
}
