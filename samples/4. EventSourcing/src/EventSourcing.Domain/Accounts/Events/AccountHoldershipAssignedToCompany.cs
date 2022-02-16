using Brickweave.EventStore;
using EventSourcing.Domain.Companies.Models;

namespace EventSourcing.Domain.Accounts.Events
{
    public class AccountHoldershipAssignedToCompany : IEvent
    {
        public AccountHoldershipAssignedToCompany(CompanyId companyId)
        {
            CompanyId = companyId;
        }

        public CompanyId CompanyId { get; }
    }
}
