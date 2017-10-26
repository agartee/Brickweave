using System;
using System.Collections.Generic;
using System.Linq;

namespace Brickweave.EventStore.SqlServer.Tests.Models
{
    public class TestAggregate : EventSourcedAggregateRoot
    {
        TestAggregate()
        {
            Register<TestAggregateCreated>(Apply);
        }

        public TestAggregate(IEnumerable<IAggregateEvent> events) : this()
        {
            events.ToList().ForEach(ApplyEvent);
        }

        public TestAggregate(Guid testId) : this()
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
