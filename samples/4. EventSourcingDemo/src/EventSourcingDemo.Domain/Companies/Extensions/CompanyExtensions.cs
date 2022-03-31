using EventSourcingDemo.Domain.Companies.Models;

namespace EventSourcingDemo.Domain.Companies.Extensions
{
    public static class CompanyExtensions
    {
        public static CompanyInfo ToCompanyInfo(this Company company)
        {
            return new CompanyInfo(company.Id, company.Name);
        }
    }
}
