using Brickweave.Cqrs;
using EventSourcingDemo.Domain.People.Models;

namespace EventSourcingDemo.Domain.People.Commands
{
    public class DeletePerson : ICommand
    {
        public DeletePerson(PersonId personId)
        {
            PersonId = personId ?? throw new ArgumentNullException(nameof(personId));
        }

        public PersonId PersonId { get; }
    }
}
