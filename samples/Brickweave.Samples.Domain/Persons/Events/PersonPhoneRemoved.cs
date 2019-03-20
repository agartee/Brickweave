using System;
using Brickweave.EventStore;
using Brickweave.Samples.Domain.Phones.Models;

namespace Brickweave.Samples.Domain.Persons.Events
{
    public class PersonPhoneRemoved : IEvent
    {
        public PersonPhoneRemoved(PhoneId phoneId)
        {
            PhoneId = phoneId;
        }

        public PhoneId PhoneId { get; }
    }
}
