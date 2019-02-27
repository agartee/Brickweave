using System;
using Brickweave.EventStore;

namespace Brickweave.Samples.Domain.Persons.Events
{
    public class PersonPhoneRemoved : IEvent
    {
        public PersonPhoneRemoved(Guid phoneId)
        {
            PhoneId = phoneId;
        }

        public Guid PhoneId { get; }
    }
}
