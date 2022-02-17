using EventSourcingDemo.Domain.Common.Models;

namespace EventSourcingDemo.Domain.Companies.Models
{
    public class Company : LegalEntity<CompanyId>
    {
        public Company(CompanyId id, Name name) : base(id, name)
        {
        }
    }
}
