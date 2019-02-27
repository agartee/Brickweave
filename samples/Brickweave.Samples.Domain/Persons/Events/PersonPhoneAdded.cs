using System;
using Brickweave.EventStore;

namespace Brickweave.Samples.Domain.Persons.Events
{
    public class PersonPhoneAdded : IEvent
    {
        public PersonPhoneAdded(Guid phoneId, string number)
        {
            PhoneId = phoneId;
            Number = number;
        }

        public Guid PhoneId { get; }
        public string Number { get; }
    }
}
