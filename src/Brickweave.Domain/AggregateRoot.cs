using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Brickweave.Domain
{
    public abstract class AggregateRoot
    {
        private readonly IList<IDomainEvent> _enqueuedDomainEvents = new List<IDomainEvent>();

        protected void EnqueueDomainEvent(IDomainEvent @event)
        {
            Guard.IsNotNullDomainEvent(@event);

            _enqueuedDomainEvents.Add(@event);
        }

        public void ClearEnqueuedDomainEvents()
        {
            _enqueuedDomainEvents.Clear();
        }

        public IEnumerable<IDomainEvent> GetEnqueuedDomainEvents()
        {
            return new ReadOnlyCollection<IDomainEvent>(_enqueuedDomainEvents);
        }

        private static class Guard
        {
            public static void IsNotNullDomainEvent(IDomainEvent @event)
            {
                if (@event == null)
                    throw new ArgumentNullException("Cannot enqueue a null domain event.");
            }
        }
    }
}
