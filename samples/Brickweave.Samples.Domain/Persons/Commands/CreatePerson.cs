using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class CreatePerson : ICommand<PersonInfo>
    {
        public CreatePerson(PersonId personId, Name name)
        {
            PersonId = personId;
            Name = name;
        }

        public PersonId PersonId { get; }
        public Name Name { get; }
    }
}
