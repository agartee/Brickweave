using Brickweave.Cqrs;
using EventSourcingDemo.Domain.Common.Models;
using EventSourcingDemo.Domain.Companies.Models;

namespace EventSourcingDemo.Domain.Companies.Commands
{
    public class UpdateCompany : ICommand<CompanyInfo>
    {
        public UpdateCompany(CompanyId companyId, Name name)
        {
            CompanyId = companyId ?? throw new ArgumentNullException(nameof(companyId));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public CompanyId CompanyId { get; }
        public Name Name { get; }
    }
}
