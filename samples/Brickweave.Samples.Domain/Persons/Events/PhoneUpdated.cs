using Brickweave.EventStore;
using System;

namespace Brickweave.Samples.Domain.Persons.Events
{
    public class PhoneUpdated : IChildEvent
    {
        public PhoneUpdated(Guid id, string number)
        {
            Id = id;
            Number = number;
        }

        public Guid Id { get; }
        public string Number { get; }

        public object GetEntityId() => Id;
    }
}
