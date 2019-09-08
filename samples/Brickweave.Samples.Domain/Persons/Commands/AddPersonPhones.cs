using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;
using System.Collections.Generic;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class AddPersonPhones : ICommand<PersonInfo>
    {
        /// <summary>
        /// Add phone numbers to an existing person.
        /// </summary>
        /// <param name="personId">person id</param>
        /// <param name="phoneNumbers">phone number</param>
        public AddPersonPhones(PersonId personId, IEnumerable<string> phoneNumbers)
        {
            Id = personId;
            PhoneNumbers = phoneNumbers;
        }

        public PersonId Id { get; }
        public IEnumerable<string> PhoneNumbers { get; }
    }
}
