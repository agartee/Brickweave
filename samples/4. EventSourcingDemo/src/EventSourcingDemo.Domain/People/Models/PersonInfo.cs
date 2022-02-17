using EventSourcingDemo.Domain.Common.Models;

namespace EventSourcingDemo.Domain.People.Models
{
    public class PersonInfo : LegalEntityInfo
    {
        public PersonInfo(PersonId id, Name name) : base(id, name)
        {
        }
    }
}
