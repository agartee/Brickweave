using System.Collections.Generic;

namespace Brickweave.EventStore
{
    public abstract class EventSourcedAggregateRoot : EventSourcedEntity
    {
        protected EventSourcedAggregateRoot() : base(new Queue<IEvent>(), new RegistrationEventRouter())
        {

        }

        public IEnumerable<IEvent> GetUncommittedEvents()
        {
            return EventQueue.ToArray();
        }

        public void ClearUncommittedEvents()
        {
            EventQueue.Clear();
        }
    }
}
