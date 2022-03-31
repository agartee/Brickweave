using Brickweave.Cqrs;
using EventSourcingDemo.Domain.Companies.Models;

namespace EventSourcingDemo.Domain.Companies.Commands
{
    public class DeleteCompany : ICommand
    {
        public DeleteCompany(CompanyId companyId)
        {
            CompanyId = companyId ?? throw new ArgumentNullException(nameof(companyId));
        }

        public CompanyId CompanyId { get; }
    }
}
