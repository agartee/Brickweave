﻿using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Phones.Models;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class RemovePersonPhone : ICommand<PersonInfo>
    {
        /// <summary>
        /// Remove a phone number from an existing person.
        /// </summary>
        /// <param name="personId">Existing person's ID</param>
        /// <param name="phoneId">Existing phone's ID</param>
        public RemovePersonPhone(PersonId personId, PhoneId phoneId)
        {
            Id = personId;
            PhoneId = phoneId;
        }

        public PersonId Id { get; }
        public PhoneId PhoneId { get; }
    }
}