using System;
using BasicCqrs.Domain.People.Models;
using Brickweave.Cqrs;

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
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        public PersonId Id { get; }
    }
}
