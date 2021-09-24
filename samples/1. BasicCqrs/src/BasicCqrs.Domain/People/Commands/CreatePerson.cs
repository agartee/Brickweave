using BasicCqrs.Domain.People.Models;
using Brickweave.Cqrs;
using LiteGuard;

namespace BasicCqrs.Domain.People.Commands
{
    public class CreatePerson : ICommand<Person>
    {
        /// <summary>
        /// Creates a new Person.
        /// </summary>
        /// <param name="firstName">New person's first name.</param>
        /// <param name="lastName">New person's last name.</param>
        public CreatePerson(string firstName, string lastName)
        {
            Guard.AgainstNullArgument(nameof(firstName), firstName);
            Guard.AgainstNullArgument(nameof(lastName), lastName);

            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; }
        public string LastName { get; }
    }
}
