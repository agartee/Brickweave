namespace Brickweave.Samples.Domain.Persons.Models
{
    public class PersonInfo
    {
        public PersonInfo(PersonId id, Name name)
        {
            Id = id;
            Name = name;
        }

        public PersonId Id { get; }
        public Name Name { get; }
    }
}
