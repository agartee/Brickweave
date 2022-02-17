using EventSourcing.Domain.Common.Models;

namespace EventSourcing.Domain.People.Models
{
    public class Person : LegalEntity
    {
        public Person(PersonId id, Name name) : base(id, name)
        {
        }
    }
}
