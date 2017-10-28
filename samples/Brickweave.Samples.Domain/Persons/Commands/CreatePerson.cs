using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class CreatePerson : ICommand<PersonInfo>
    {
        public CreatePerson(PersonId id, Name name)
        {
            Id = id;
            Name = name;
        }

        public CreatePerson(PersonId id, string firstName, string lastName)
        {
            Id = id;
            Name = new Name(firstName, lastName);
        }

        public PersonId Id { get; }
        public Name Name { get; }
    }
}
