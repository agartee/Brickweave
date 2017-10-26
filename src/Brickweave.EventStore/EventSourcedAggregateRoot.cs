using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Brickweave.EventStore
{
    public abstract class EventSourcedAggregateRoot
    {
        private readonly IEventRouter _router = new RegistrationEventRouter();
        private readonly LinkedList<IAggregateEvent> _uncommittedEvents = new LinkedList<IAggregateEvent>();

        protected void ApplyEvent(IAggregateEvent @event)
        {
            _router.Dispatch(@event, GetType());
        }

        protected void ApplyEvents(IEnumerable<IAggregateEvent> events)
        {
            events.ToList().ForEach(ApplyEvent);
        }

        public IEnumerable<IAggregateEvent> GetUncommittedEvents()
        {
            return new ReadOnlyCollection<IAggregateEvent>(_uncommittedEvents.ToList());
        }

        public void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        protected void Register<THandler>(Action<THandler> route)
        {
            _router.Register(route);
        }

        protected void RaiseEvent(IAggregateEvent aggregateEvent)
        {
            ApplyEvent(aggregateEvent);
            _uncommittedEvents.AddLast(aggregateEvent);
        }
    }
}
