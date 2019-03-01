using System;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class CreatePerson : ICommand<PersonInfo>
    {
        /// <summary>
        /// Create a new person.
        /// </summary>
        /// <param name="firstName">Person's first name</param>
        /// <param name="lastName">Person's last name</param>
        /// <param name="birthDate">Person's birth date</param>
        public CreatePerson(string firstName, string lastName, DateTime birthDate)
        {
            Id = PersonId.NewId();
            Name = new Name(firstName, lastName);
            BirthDate = birthDate;
        }

        public PersonId Id { get; }
        public Name Name { get; }
        public DateTime BirthDate { get; }
    }
}
