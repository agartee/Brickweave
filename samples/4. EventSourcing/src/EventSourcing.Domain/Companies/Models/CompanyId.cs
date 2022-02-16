using EventSourcing.Domain.Common.Models;

namespace EventSourcing.Domain.Companies.Models
{
    public class CompanyId : LegalEntityId
    {
        public CompanyId(Guid value) : base(value)
        {
        }

        public static CompanyId NewId()
        {
            return new CompanyId(Guid.NewGuid());
        }
    }
}
