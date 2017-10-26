using System;
using System.Collections.Generic;
using System.Reflection;
using Brickweave.EventStore.Exceptions;

namespace Brickweave.EventStore.Factories
{
    public class ReflectionConventionAggregateFactory : IAggregateFactory
    {
        public T Create<T>(IEnumerable<IAggregateEvent> events) where T : class
        {
            var type = typeof(T);
            var ctor = type.GetTypeInfo().GetConstructor(new[] { typeof(IEnumerable<IAggregateEvent>) });

            if (ctor == null)
                throw new ConstructorNotFoundException(type, new[] { typeof(IEnumerable<IAggregateEvent>) });

            return (T)ctor.Invoke(new object[] { events });
        }

        public object Create(string aggregateType, IEnumerable<IAggregateEvent> events)
        {
            var type = Type.GetType(aggregateType);

            if (type == null)
                throw new UnrecognizedTypeException(aggregateType);

            var ctor = type.GetTypeInfo().GetConstructor(new[] { typeof(IEnumerable<IAggregateEvent>) });
            if (ctor == null)
                throw new ConstructorNotFoundException(type, new[] { typeof(IEnumerable<IAggregateEvent>) });

            return ctor.Invoke(new object[] { events });
        }
    }
}
