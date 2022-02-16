using EventSourcing.Domain.Common.Models;

namespace EventSourcing.Domain.People.Models
{
    public class PersonId : LegalEntityId
    {
        public PersonId(Guid value) : base(value)
        {
        }

        public static PersonId NewId()
        {
            return new PersonId(Guid.NewGuid());
        }
    }
}
