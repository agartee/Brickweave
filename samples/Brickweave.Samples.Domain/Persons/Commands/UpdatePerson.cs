using System;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class UpdatePerson : ICommand<PersonInfo>
    {
        /// <summary>
        /// Update an existing person.
        /// </summary>
        /// <param name="personId">Existing person's ID</param>
        /// <param name="firstName">New first name. Unspecified values will result in no change.</param>
        /// <param name="lastName">New last name. Unspecified values will result in no change.</param>
        /// <param name="birthDate">New birth date. Unspecified values will result in no change.</param>
        public UpdatePerson(PersonId personId, string firstName, string lastName, DateTime? birthDate)
        {
            PersonId = personId;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }

        public PersonId PersonId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public DateTime? BirthDate { get; }
    }
}
