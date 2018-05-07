using System;
using System.Collections.Generic;
using System.Linq;

namespace Brickweave.EventStore
{
    public abstract class EventSourcedEntity
    {
        protected EventSourcedEntity(Queue<IEvent> eventQueue, IEventRouter router)
        {
            EventQueue = eventQueue;
            Router = router;
        }

        protected IEventRouter Router { get; }
        protected Queue<IEvent> EventQueue { get; }

        protected void Register<THandler>(Action<THandler> route, object id = null)
        {
            Router.Register(route, id);
        }

        protected void RaiseEvent(IEvent @event)
        {
            ApplyEvent(@event);
            EventQueue.Enqueue(@event);
        }

        protected void ApplyEvent(IEvent @event)
        {
            Router.Dispatch(@event, GetType(),
                @event is IChildEvent ? ((IChildEvent) @event).GetEntityId() : null); 
        }

        protected void ApplyEvents(IEnumerable<IEvent> events)
        {
            events.ToList().ForEach(e => ApplyEvent(e));
        }
    }
}
