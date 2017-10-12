using System;

namespace Brickweave.EventStore.Exceptions
{
    public class EventHandlerNotFoundException : Exception
    {
        private const string MESSAGE = "Handler not found for event type, \"{0}\"{1}.";
        private const string MESSAGE_AGGREGATETYPE = " in aggregate type, \"{0}\"";

        public EventHandlerNotFoundException(Type eventType, Type aggregateType = null)
            : base(string.Format(MESSAGE, eventType,
                aggregateType != null ? string.Format(MESSAGE_AGGREGATETYPE, aggregateType) : null))
        {
            EventType = eventType;
            AggregateType = aggregateType;
        }

        public Type EventType { get; }
        public Type AggregateType { get; }
    }
}
