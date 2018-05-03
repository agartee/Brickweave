using System;

namespace Brickweave.EventStore.SqlServer.Tests.Models
{
    public class TestAggregateCreated : IEvent
    {
        public TestAggregateCreated(Guid testId)
        {
            TestId = testId;
        }

        public Guid TestId { get; }
    }
}