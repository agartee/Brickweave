using System.Collections.Generic;
using System.Linq;

namespace Brickweave.EventStore.SqlServer.Tests.Models
{
    public class TestAggregate : EventSourcedAggregateRoot<TestId>
    {
        TestAggregate()
        {
            Register<TestAggregateCreated>(Apply);
        }

        public TestAggregate(IEnumerable<IAggregateEvent> events) : this()
        {
            events.ToList().ForEach(ApplyEvent);
        }

        public TestAggregate(TestId testId) : this()
        {
            RaiseEvent(new TestAggregateCreated(testId.Value));
        }

        public TestId TestId { get; private set; }

        private void Apply(TestAggregateCreated @event)
        {
            TestId = new TestId(@event.TestId);
        }
    }
}
