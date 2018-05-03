using System;

namespace Brickweave.EventStore
{
    public interface IEventRouter
    {
        void Register<T>(Action<T> handler, object id = null);
        void Dispatch(object @event, Type aggregateType, object id = null);
    }
}