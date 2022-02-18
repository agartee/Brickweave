using Brickweave.Cqrs;
using EventSourcingDemo.Domain.Companies.Models;
using EventSourcingDemo.Domain.Companies.Services;

namespace EventSourcingDemo.Domain.Companies.Queries
{
    public class ListCompaniesHandler : IQueryHandler<ListCompanies, IEnumerable<CompanyInfo>>
    {
        private readonly ICompanyRepository _companyRepository;

        public ListCompaniesHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<IEnumerable<CompanyInfo>> HandleAsync(ListCompanies query)
        {
            var results = await _companyRepository.ListCompaniesAsync();

            return results
                .OrderBy(p => p.Name)
                .ToList();
        }
    }
}
