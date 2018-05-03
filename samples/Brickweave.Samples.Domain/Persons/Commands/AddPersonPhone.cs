using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class AddPersonPhone : ICommand<PersonInfo>
    {
        /// <summary>
        /// Add a phone to a person
        /// </summary>
        /// <param name="id">person id</param>
        /// <param name="phoneNumber">phone number</param>
        public AddPersonPhone(PersonId id, string phoneNumber)
        {
            Id = id;
            PhoneNumber = phoneNumber;
        }

        public PersonId Id { get; }
        public string PhoneNumber { get; }
    }
}
