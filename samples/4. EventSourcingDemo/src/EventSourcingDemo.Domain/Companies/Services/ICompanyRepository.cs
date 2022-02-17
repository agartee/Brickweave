using EventSourcingDemo.Domain.Companies.Models;

namespace EventSourcingDemo.Domain.Companies.Services
{
    public interface ICompanyRepository
    {
        Task SaveCompanyAsync(Company company);
        Task<Company> DemandCompanyAsync(CompanyId id);
        Task<IEnumerable<Company>> ListCompaniesAsync();
        Task DeleteCompanyAsync(CompanyId id);
    }
}
