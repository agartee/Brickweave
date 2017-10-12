namespace Brickweave.EventStore.Tests.Models
{
    public class TestAggregateWithoutEventRegistrations : EventSourcedAggregateRoot<TestId>
    {
        public TestAggregateWithoutEventRegistrations(TestId testId)
        {
            RaiseEvent(new TestAggregateCreated(testId.Value));
        }
    }
}