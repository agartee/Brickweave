using System.Collections.Generic;

namespace Brickweave.Samples.Domain.Persons.Models
{
    public class PersonInfo
    {
        public PersonInfo(PersonId id, Name name, IEnumerable<PhoneInfo> phones)
        {
            Id = id;
            Name = name;
            Phones = phones;
        }

        public PersonId Id { get; }
        public Name Name { get; }
        public IEnumerable<PhoneInfo> Phones { get; }
    }
}
