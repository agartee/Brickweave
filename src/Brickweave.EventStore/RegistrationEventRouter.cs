using System;
using System.Collections.Generic;
using System.Linq;
using Brickweave.EventStore.Exceptions;

namespace Brickweave.EventStore
{
    public class RegistrationEventRouter : IEventRouter
    {
        private readonly IDictionary<Type, Action<object>> _handlers
            = new Dictionary<Type, Action<object>>();

        public void Register<T>(Action<T> handler)
        {
            _handlers[typeof(T)] = @event => handler((T)@event);
        }

        public void Dispatch(object @event, Type aggregateType = null)
        {
            if (!_handlers.Any())
                throw new EventHandlersNoFoundException(aggregateType);

            if (!_handlers.TryGetValue(@event.GetType(), out Action<object> handler))
                throw new EventHandlerNotFoundException(@event.GetType(), aggregateType);

            handler(@event);
        }
    }
}