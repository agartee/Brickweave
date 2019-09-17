using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Phones.Models;
using System;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class UpdatePersonPhone : ICommand<PersonInfo>
    {
        /// <summary>
        /// Update an existing person's phone number.
        /// </summary>
        /// <param name="personId">Existing person's ID</param>
        /// <param name="phoneId">Existing phone's ID</param>
        /// <param name="type">New phone type. Unspecified values will result in no change.</param>
        /// <param name="number">New phone number. Unspecified values will result in no change.</param>
        public UpdatePersonPhone(PersonId personId, PhoneId phoneId, string type, string number)
        {
            PersonId = personId;
            PhoneId = phoneId;

            Enum.TryParse(type, true, out PhoneType phoneType);
            PhoneType = phoneType;

            Number = number;
        }

        public PersonId PersonId { get; }
        public PhoneId PhoneId { get; }
        public PhoneType? PhoneType { get; }
        public string Number { get; }
    }
}
