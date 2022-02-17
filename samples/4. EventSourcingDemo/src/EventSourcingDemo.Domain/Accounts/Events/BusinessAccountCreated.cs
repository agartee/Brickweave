using Brickweave.Domain;
using Brickweave.EventStore;
using EventSourcingDemo.Domain.Accounts.Models;
using EventSourcingDemo.Domain.Common.Models;
using EventSourcingDemo.Domain.Companies.Models;

namespace EventSourcingDemo.Domain.Accounts.Events
{
    public class BusinessAccountCreated : IEvent, IDomainEvent
    {
        public BusinessAccountCreated(AccountId accountId, Name accountName, CompanyId accountHolderId)
        {
            AccountId = accountId ?? throw new ArgumentNullException(nameof(accountId));
            AccountName = accountName ?? throw new ArgumentNullException(nameof(accountName));
            AccountHolderId = accountHolderId ?? throw new ArgumentNullException(nameof(accountHolderId));
        }

        public AccountId AccountId { get; }
        public Name AccountName { get; }
        public CompanyId AccountHolderId { get; }
    }
}
