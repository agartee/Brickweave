using System;

namespace Brickweave.EventStore.SqlServer.Tests.Models
{
    public class TestAggregateCreated : IEvent
    {
        public TestAggregateCreated(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}