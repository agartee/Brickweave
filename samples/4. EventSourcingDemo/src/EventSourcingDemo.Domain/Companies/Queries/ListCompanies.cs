using Brickweave.Cqrs;
using EventSourcingDemo.Domain.Companies.Models;

namespace EventSourcingDemo.Domain.Companies.Queries
{
    public class ListCompanies : IQuery<IEnumerable<CompanyInfo>>
    {
    }
}
