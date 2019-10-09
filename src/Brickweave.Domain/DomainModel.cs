using System.Collections.Generic;
using LiteGuard;

namespace Brickweave.Domain
{
    public class DomainModel
    {
        protected DomainModel(Queue<IDomainEvent> domainEventQueue)
        {
            Guard.AgainstNullArgument(nameof(domainEventQueue), domainEventQueue);

            DomainEventQueue = domainEventQueue;
        }

        protected Queue<IDomainEvent> DomainEventQueue { get; }

        protected void RaiseEvent(IDomainEvent @event)
        {
            DomainEventQueue.Enqueue(@event);
        }

        public IEnumerable<IDomainEvent> GetDomainEvents()
        {
            return DomainEventQueue.ToArray();
        }
    }
}
