using EventSourcing.Domain.Common.Models;

namespace EventSourcing.Domain.Companies.Models
{
    public class Company : LegalEntity<CompanyId>
    {
        public Company(CompanyId id, Name name) : base(id, name)
        {
        }
    }
}
