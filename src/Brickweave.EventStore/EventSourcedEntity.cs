using System;
using System.Collections.Generic;
using System.Linq;
using Brickweave.Domain;
using LiteGuard;

namespace Brickweave.EventStore
{
    public abstract class EventSourcedEntity : DomainModel
    {
        protected EventSourcedEntity(Queue<IEvent> eventQueue, Queue<IDomainEvent> domainEventQueue, IEventRouter eventRouter)
            : base(domainEventQueue)
        {
            Guard.AgainstNullArgument(nameof(eventQueue), eventQueue);
            Guard.AgainstNullArgument(nameof(eventRouter), eventRouter);

            EventQueue = eventQueue;
            EventRouter = eventRouter;
        }

        protected Queue<IEvent> EventQueue { get; }
        protected IEventRouter EventRouter { get; }

        protected void Register<THandler>(Action<THandler> route, object id = null)
        {
            EventRouter.Register(route, id);
        }

        protected void RaiseEvent(IEvent @event)
        {
            ApplyEvent(@event);
            EventQueue.Enqueue(@event);

            if(@event is IDomainEvent domainEvent)
                RaiseEvent(domainEvent);
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
