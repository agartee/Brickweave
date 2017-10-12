using System;
using Brickweave.Domain;

namespace Brickweave.EventStore.Tests.Models
{
    public class TestAggregateCreated : IAggregateEvent, IDomainEvent
    {
        public TestAggregateCreated(Guid testId)
        {
            TestId = testId;
        }

        public Guid TestId { get; }
    }
}