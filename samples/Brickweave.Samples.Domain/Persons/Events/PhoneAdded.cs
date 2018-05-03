using System;
using Brickweave.EventStore;

namespace Brickweave.Samples.Domain.Persons.Events
{
    public class PhoneAdded : IEvent
    {
        public PhoneAdded(Guid id, string number)
        {
            Id = id;
            Number = number;
        }

        public Guid Id { get; }
        public string Number { get; }
    }
}
