using System;

namespace Brickweave.EventStore.Tests.Models
{
    public class TestAggregateCreated : IAggregateEvent
    {
        public TestAggregateCreated(Guid testId)
        {
            TestId = testId;
        }

        public Guid TestId { get; }
    }
}