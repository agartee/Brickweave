using System.Collections.Generic;
using Brickweave.EventStore;
using Brickweave.Samples.Domain.Phones.Events;

namespace Brickweave.Samples.Domain.Phones.Models
{
    public class Phone : EventSourcedEntity
    {
        public Phone(PhoneId id, string number, Queue<IEvent> eventQueue, IEventRouter router)
            : base(eventQueue, router)
        {
            Id = id;
            Number = number;

            Register<PhoneUpdated>(Apply, id);
        }
        
        public PhoneId Id { get; private set; }
        public string Number { get; private set; }

        public void UpdateNumber(string number)
        {
            RaiseEvent(new PhoneUpdated(Id.Value, number));
        }

        private void Apply(PhoneUpdated @event)
        {
            Number = @event.Number;
        }
    }
}
