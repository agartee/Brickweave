using System.Collections.Generic;
using Brickweave.Domain;

namespace Brickweave.EventStore.Factories
{
    public interface IAggregateFactory
    {
        T Create<T>(IEnumerable<IAggregateEvent> events) where T : AggregateRoot;
        AggregateRoot Create(string aggregateType, IEnumerable<IAggregateEvent> events);
    }
}