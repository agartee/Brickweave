using EventSourcingDemo.Domain.Common.Models;

namespace EventSourcingDemo.Domain.People.Models
{
    public class Person : LegalEntity<PersonId>
    {
        public Person(PersonId id, Name name) : base(id, name)
        {
        }
    }
}
