using System;

namespace Brickweave.EventStore
{
    public interface IEventRouter
    {
        void Register<T>(Action<T> handler);
        void Dispatch(object @event, Type aggregateType = null);
    }
}