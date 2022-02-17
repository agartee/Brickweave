using Brickweave.Domain;
using Brickweave.EventStore;
using EventSourcingDemo.Domain.Accounts.Models;

namespace EventSourcingDemo.Domain.Accounts.Events
{
    public class AccountDeleted : IEvent, IDomainEvent
    {
        public AccountDeleted(AccountId accountId)
        {
            AccountId = accountId ?? throw new ArgumentNullException(nameof(accountId));
        }

        public AccountId AccountId { get; }
    }
}
