using Brickweave.Cqrs;
using EventSourcingDemo.Domain.Companies.Models;

namespace EventSourcingDemo.Domain.Companies.Queries
{
    public class GetCompany : IQuery<CompanyInfo>
    {
        public GetCompany(CompanyId companyId)
        {
            CompanyId = companyId ?? throw new ArgumentNullException(nameof(companyId));
        }

        public CompanyId CompanyId { get; }
    }
}
