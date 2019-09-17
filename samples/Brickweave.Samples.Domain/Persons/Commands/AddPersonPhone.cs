using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Phones.Models;
using System;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class AddPersonPhone : ICommand<PersonInfo>
    {
        /// <summary>
        /// Add phone numbers to an existing person.
        /// </summary>
        /// <param name="personId">Existing person's ID</param>
        /// <param name="type">New phone type (allowed values: Home, Work)</param>
        /// <param name="number">New phone number</param>
        public AddPersonPhone(PersonId personId, string type, string number)
        {
            Id = personId;
            PhoneType = (PhoneType) Enum.Parse(typeof(PhoneType), type, true);
            Number = number;
        }

        public PersonId Id { get; }
        public PhoneType PhoneType { get; }
        public string Number { get; }
    }
}
