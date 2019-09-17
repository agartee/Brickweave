using System.Collections.Generic;
using Brickweave.EventStore;
using Brickweave.Samples.Domain.Phones.Events;

namespace Brickweave.Samples.Domain.Phones.Models
{
    public class Phone : EventSourcedEntity
    {
        Phone(PhoneId id, Queue<IEvent> eventQueue, IEventRouter router)
            : base(eventQueue, router)
        {
            Register<PhoneTypeUpdated>(Apply, id);
            Register<PhoneNumberUpdated>(Apply, id);
        }

        public Phone(PhoneId id, PhoneType phoneType, string number, Queue<IEvent> eventQueue, IEventRouter router)
            : this(id, eventQueue, router)
        {
            Id = id;
            PhoneType = phoneType;
            Number = number;
        }
        
        public PhoneId Id { get; private set; }
        public PhoneType PhoneType { get; private set; }
        public string Number { get; private set; }

        public void UpdateType(PhoneType phoneType)
        {
            RaiseEvent(new PhoneTypeUpdated(Id.Value, phoneType));
        }

        public void UpdateNumber(string number)
        {
            RaiseEvent(new PhoneNumberUpdated(Id.Value, number));
        }

        private void Apply(PhoneTypeUpdated @event)
        {
            PhoneType = @event.PhoneType;
        }

        private void Apply(PhoneNumberUpdated @event)
        {
            Number = @event.Number;
        }
    }
}
