using Brickweave.Domain;
using Brickweave.EventStore;
using EventSourcing.Domain.Accounts.Models;
using EventSourcing.Domain.Common.Models;
using EventSourcing.Domain.People.Models;

namespace EventSourcing.Domain.Accounts.Events
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
