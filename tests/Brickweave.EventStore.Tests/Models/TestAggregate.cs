using System.Collections.Generic;
using System.Linq;

namespace Brickweave.EventStore.Tests.Models
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

        public void RaiseUnregisteredEvent()
        {
            RaiseEvent(new UnregisteredEvent());
        }

        private void Apply(TestAggregateCreated @event)
        {
            TestId = new TestId(@event.TestId);
        }
    }
}
