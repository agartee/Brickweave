using System;
using Brickweave.EventStore;
using Brickweave.Samples.Domain.Phones.Models;

namespace Brickweave.Samples.Domain.Phones.Events
{
    public class PhoneTypeUpdated : IChildEvent
    {
        public PhoneTypeUpdated(Guid phoneId, PhoneType phoneType)
        {
            PhoneId = phoneId;
            PhoneType = phoneType;
        }

        public Guid PhoneId { get; }
        public PhoneType PhoneType { get; }

        public object GetEntityId() => PhoneId;
    }
}
