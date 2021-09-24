using System.ComponentModel.DataAnnotations;
using BasicCqrs.Domain.People.Models;
using Brickweave.Cqrs;

namespace BasicCqrs.Domain.People.Commands
{
    /// <summary>
    /// Updates an existing Person.
    /// </summary>
    public class UpdatePerson : ICommand<Person>
    {
        /// <summary>
        /// Existing person's unique identifier.
        /// </summary>
        [Required]
        public PersonId Id { get; init; }

        /// <summary>
        /// Existing person's new first name.
        /// </summary>
        public string FirstName { get; init; }

        /// <summary>
        /// Existing person's new last name.
        /// </summary>
        public string LastName { get; init; }
    }
}
