using System;
using BasicCqrs.Domain.People.Models;
using Brickweave.Cqrs;

namespace BasicCqrs.Domain.People.Commands
{
    public class CreatePerson : ICommand<Person>
    {
        /// <summary>
        /// Creates a new Person. Note that this class is initialized via a 
        /// constructor, allowing for more advanced validation should it be
        /// required.
        /// </summary>
        /// <param name="firstName">New person's first name.</param>
        /// <param name="lastName">New person's last name.</param>
        public CreatePerson(string firstName, string lastName)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        }

        public string FirstName { get; }
        public string LastName { get; }
    }
}
