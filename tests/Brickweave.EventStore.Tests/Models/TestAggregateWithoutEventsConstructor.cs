using System;

namespace Brickweave.EventStore.Tests.Models
{
    public class TestAggregateWithoutEventsConstructor : EventSourcedAggregateRoot
    {
        TestAggregateWithoutEventsConstructor()
        {
            Register<TestAggregateCreated>(Apply);
        }
        
        public TestAggregateWithoutEventsConstructor(Guid testId) : this()
        {
            RaiseEvent(new TestAggregateCreated(testId));
        }

        public Guid TestId { get; private set; }

        private void Apply(TestAggregateCreated @event)
        {
            TestId = @event.TestId;
        }
    }
}