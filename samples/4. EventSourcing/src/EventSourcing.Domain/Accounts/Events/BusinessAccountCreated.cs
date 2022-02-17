using Brickweave.Domain;
using Brickweave.EventStore;
using EventSourcing.Domain.Accounts.Models;
using EventSourcing.Domain.Common.Models;
using EventSourcing.Domain.Companies.Models;

namespace EventSourcing.Domain.Accounts.Events
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
