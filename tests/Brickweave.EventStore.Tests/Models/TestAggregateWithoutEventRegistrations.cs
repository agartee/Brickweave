using System;

namespace Brickweave.EventStore.Tests.Models
{
    public class TestAggregateWithoutEventRegistrations : EventSourcedAggregateRoot
    {
        public TestAggregateWithoutEventRegistrations(Guid testId)
        {
            RaiseEvent(new TestAggregateCreated(testId));
        }
    }
}