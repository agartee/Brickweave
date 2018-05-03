using System.Collections.Generic;

namespace Brickweave.EventStore.Factories
{
    public interface IAggregateFactory
    {
        T Create<T>(IEnumerable<IEvent> events) where T : class;
        object Create(string aggregateType, IEnumerable<IEvent> events);
    }
}