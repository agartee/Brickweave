using System;
using System.Collections.Generic;
using System.Reflection;
using Brickweave.EventStore.Exceptions;

namespace Brickweave.EventStore.Factories
{
    public class ReflectionConventionAggregateFactory : IAggregateFactory
    {
        public T Create<T>(IEnumerable<IEvent> events) where T : class
        {
            var type = typeof(T);
            var ctor = type.GetTypeInfo().GetConstructor(new[] { typeof(IEnumerable<IEvent>) });

            if (ctor == null)
                throw new ConstructorNotFoundException(type, new[] { typeof(IEnumerable<IEvent>) });

            return (T)ctor.Invoke(new object[] { events });
        }

        public object Create(string aggregateType, IEnumerable<IEvent> events)
        {
            var type = Type.GetType(aggregateType);

            if (type == null)
                throw new UnrecognizedTypeException(aggregateType);

            var ctor = type.GetTypeInfo().GetConstructor(new[] { typeof(IEnumerable<IEvent>) });
            if (ctor == null)
                throw new ConstructorNotFoundException(type, new[] { typeof(IEnumerable<IEvent>) });

            return ctor.Invoke(new object[] { events });
        }
    }
}
