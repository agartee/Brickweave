using Brickweave.Cqrs;
using EventSourcingDemo.Domain.Companies.Extensions;
using EventSourcingDemo.Domain.Companies.Models;
using EventSourcingDemo.Domain.Companies.Services;

namespace EventSourcingDemo.Domain.Companies.Queries
{
    public class GetCompanyHandler : IQueryHandler<GetCompany, CompanyInfo>
    {
        private readonly ICompanyRepository _companyRepository;

        public GetCompanyHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CompanyInfo> HandleAsync(GetCompany query)
        {
            var result = await _companyRepository.DemandCompanyAsync(query.CompanyId);

            return result.ToCompanyInfo();
        }
    }
}
