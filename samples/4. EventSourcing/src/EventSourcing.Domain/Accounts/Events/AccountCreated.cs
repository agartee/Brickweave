using Brickweave.EventStore;
using EventSourcing.Domain.Accounts.Models;
using EventSourcing.Domain.Common.Models;

namespace EventSourcing.Domain.Accounts.Events
{
    public class AccountCreated : IEvent
    {
        public AccountCreated(AccountId accountId, Name name)
        {
            AccountId = accountId;
            Name = name;
        }

        public AccountId AccountId { get; }
        public Name Name { get; }
    }
}
