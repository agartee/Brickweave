using System;
using Brickweave.Domain;

namespace Brickweave.Messaging.SqlServer.Tests.Models
{
    public class TestDomainEvent : IDomainEvent
    {
        public TestDomainEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
