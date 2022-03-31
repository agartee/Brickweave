using EventSourcingDemo.Domain.Common.Models;

namespace EventSourcingDemo.Domain.Companies.Models
{
    public class CompanyInfo : LegalEntityInfo
    {
        public CompanyInfo(CompanyId id, Name name) : base(id, name)
        {
        }
    }
}
