using BasicCqrs.Domain.People.Models;
using Brickweave.Cqrs;
using LiteGuard;

namespace BasicCqrs.Domain.People.Commands
{
    public class DeletePerson : ICommand
    {
        /// <summary>
        /// Updates an existing Person.
        /// </summary>
        /// <param name="id">Existing person's unique identifier.</param>
        public DeletePerson(PersonId id)
        {
            Guard.AgainstNullArgument(nameof(id), id);

            Id = id;
        }

        public PersonId Id { get; }
    }
}
