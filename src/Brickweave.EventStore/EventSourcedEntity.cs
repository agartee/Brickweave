using System;
using System.Collections.Generic;
using System.Linq;

namespace Brickweave.EventStore
{
    public abstract class EventSourcedEntity
    {
        protected EventSourcedEntity(Queue<IEvent> eventQueue, IEventRouter eventRouter)
        {
            EventQueue = eventQueue;
            EventRouter = eventRouter;
        }

        protected IEventRouter EventRouter { get; }
        protected Queue<IEvent> EventQueue { get; }

        protected void Register<THandler>(Action<THandler> route, object id = null)
        {
            EventRouter.Register(route, id);
        }

        protected void RaiseEvent(IEvent @event)
        {
            ApplyEvent(@event);
            EventQueue.Enqueue(@event);
        }

        protected void ApplyEvent(IEvent @event)
        {
            EventRouter.Dispatch(@event, GetType(),
                @event is IChildEvent ? ((IChildEvent) @event).GetEntityId() : null); 
        }

        protected void ApplyEvents(IEnumerable<IEvent> events)
        {
            events.ToList().ForEach(e => ApplyEvent(e));
        }
    }
}
