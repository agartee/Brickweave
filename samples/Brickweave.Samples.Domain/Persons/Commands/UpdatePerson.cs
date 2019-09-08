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
        /// <param name="personId">Person's ID</param>
        /// <param name="firstName">Person's first name</param>
        /// <param name="lastName">Person's last name</param>
        /// <param name="birthDate">Person's birth date</param>
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
