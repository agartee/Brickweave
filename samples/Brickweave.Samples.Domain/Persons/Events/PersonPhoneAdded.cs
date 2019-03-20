using System;
using Brickweave.EventStore;
using Brickweave.Samples.Domain.Phones.Models;

namespace Brickweave.Samples.Domain.Persons.Events
{
    public class PersonPhoneAdded : IEvent
    {
        public PersonPhoneAdded(PhoneId phoneId, string number)
        {
            PhoneId = phoneId;
            Number = number;
        }

        public PhoneId PhoneId { get; }
        public string Number { get; }
    }
}
