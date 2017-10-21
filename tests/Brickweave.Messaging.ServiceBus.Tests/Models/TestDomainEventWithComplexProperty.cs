namespace Brickweave.Messaging.ServiceBus.Tests.Models
{
    public class TestDomainEventWithComplexProperty : IDomainEvent
    {
        public TestDomainEventWithComplexProperty(ComplexId id)
        {
            Id = id;
        }

        public ComplexId Id { get; }
    }

    public class ComplexId
    {
        public ComplexId(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}