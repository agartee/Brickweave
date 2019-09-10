using System;
using Brickweave.EventStore;
using Brickweave.Samples.Domain.Phones.Models;

namespace Brickweave.Samples.Domain.Phones.Events
{
    public class PhoneNumberUpdated : IChildEvent
    {
        public PhoneNumberUpdated(Guid phoneId, string number)
        {
            PhoneId = phoneId;
            Number = number;
        }

        public Guid PhoneId { get; }
        public string Number { get; }

        public object GetEntityId() => new PhoneId(PhoneId);
    }
}
