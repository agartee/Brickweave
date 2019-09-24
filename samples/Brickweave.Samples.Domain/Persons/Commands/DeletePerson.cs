using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class DeletePerson : ICommand
    {
        /// <summary>
        /// Delete and existing person.
        /// </summary>
        /// <param name="personId">Existing person's ID</param>
        public DeletePerson(PersonId personId)
        {
            PersonId = personId;
        }

        public PersonId PersonId { get; }
    }
}
