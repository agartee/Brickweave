using EventSourcing.Domain.Companies.Models;

namespace EventSourcing.Domain.Companies.Services
{
    public interface ICompanyRepository
    {
        Task SaveCompanyAsync(Company company);
        Task<Company> DemandCompanyAsync(CompanyId id);
        Task<IEnumerable<Company>> ListCompaniesAsync();
        Task DeleteCompany(CompanyId id);
    }
}
