using EventSourcingDemo.Domain.Common.Models;
using EventSourcingDemo.Domain.Companies.Models;
using EventSourcingDemo.SqlServer.Entities;

namespace EventSourcingDemo.SqlServer.Extensions
{
    public static class CompanyExtensions
    {
        public static Company ToCompany(this CompanyData data)
        {
            return new Company(
                new CompanyId(data.Id),
                new Name(data.Name));
        }
    }
}
