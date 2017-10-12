using System;

namespace Brickweave.EventStore.Exceptions
{
    public class EventHandlersNoFoundException : Exception
    {
        private const string MESSAGE = "Handlers not found{0}. The aggregate may have an overloaded constructor not referencing the event registration constructor.";
        private const string MESSAGE_AGGREGATETYPE = " in aggregate type, \"{0}\"";

        public EventHandlersNoFoundException(Type aggregateType) 
            : base(string.Format(MESSAGE, aggregateType != null ? string.Format(MESSAGE_AGGREGATETYPE, aggregateType) : null))
        {
            AggregateType = aggregateType;
        }

        public Type AggregateType { get; }
    }
}
