using System;
using System.Collections.Generic;
using System.Linq;
using Brickweave.EventStore.Exceptions;

namespace Brickweave.EventStore
{
    public class RegistrationEventRouter : IEventRouter
    {
        private readonly IDictionary<RegistrationKey, Action<object>> _handlers
            = new Dictionary<RegistrationKey, Action<object>>();

        public void Register<T>(Action<T> handler, object id = null)
        {
            _handlers[new RegistrationKey(typeof(T), id)] = @event => handler((T)@event);
        }

        public void Dispatch(object @event, Type aggregateType, object id = null)
        {
            if (!_handlers.Any())
                throw new EventHandlersNoFoundException(aggregateType);

            if (!_handlers.TryGetValue(new RegistrationKey(@event.GetType(), id), out Action<object> handler))
                throw new EventHandlerNotFoundException(@event.GetType(), aggregateType);

            handler(@event);
        }

        private class RegistrationKey
        {
            public RegistrationKey(Type type, object entityId)
            {
                Type = type;
                EntityId = entityId;
            }

            public Type Type { get; }
            public object EntityId { get; }

            public override bool Equals(object obj)
            {
                return obj is RegistrationKey key &&
                       EqualityComparer<object>.Default.Equals(EntityId, key.EntityId) &&
                       EqualityComparer<Type>.Default.Equals(Type, key.Type);
            }

            public override int GetHashCode()
            {
                var hashCode = 1325953389;
                hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(EntityId);
                hashCode = hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(Type);
                return hashCode;
            }
        }
    }
}