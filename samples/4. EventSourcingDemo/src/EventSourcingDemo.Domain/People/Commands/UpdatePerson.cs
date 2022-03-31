using Brickweave.Cqrs;
using EventSourcingDemo.Domain.Common.Models;
using EventSourcingDemo.Domain.People.Models;

namespace EventSourcingDemo.Domain.People.Commands
{
    public class UpdatePerson : ICommand<PersonInfo>
    {
        public UpdatePerson(PersonId personId, Name name)
        {
            PersonId = personId ?? throw new ArgumentNullException(nameof(personId));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public PersonId PersonId { get; }
        public Name Name { get; }
    }
}
