namespace Brickweave.EventStore.Tests.Models
{
    public class TestAggregateWithoutEventsConstructor : EventSourcedAggregateRoot<TestId>
    {
        TestAggregateWithoutEventsConstructor()
        {
            Register<TestAggregateCreated>(Apply);
        }
        
        public TestAggregateWithoutEventsConstructor(TestId testId) : this()
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