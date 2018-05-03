using System.Collections.Generic;

namespace Brickweave.EventStore
{
    public abstract class EventSourcedAggregateRoot : EventSourcedEntity
    {
        protected EventSourcedAggregateRoot() : base(new Queue<IEvent>(), new RegistrationEventRouter())
        {

        }
    }
}
