using System.ComponentModel.DataAnnotations;
using BasicCqrs.Domain.People.Models;
using Brickweave.Cqrs;

namespace BasicCqrs.Domain.People.Commands
{
    /// <summary>
    /// Updates an existing Person. Note that this class has no constructor 
    /// defined. This makes ASP.NET model binding simpler, but does not easily 
    /// allow for more advanced validation that might be required.
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
        [Required]
        public string FirstName { get; init; }

        /// <summary>
        /// Existing person's new last name.
        /// </summary>
        [Required]
        public string LastName { get; init; }
    }
}
