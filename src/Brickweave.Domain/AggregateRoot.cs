using System.Collections.Generic;
using System.Collections.ObjectModel;
using LiteGuard;

namespace Brickweave.Domain
{
    public abstract class AggregateRoot
    {
        private readonly IList<IDomainEvent> _enqueuedDomainEvents = new List<IDomainEvent>();

        protected void EnqueueDomainEvent(IDomainEvent @event)
        {
            Guard.AgainstNullArgument(nameof(@event), @event);

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
    }
}
