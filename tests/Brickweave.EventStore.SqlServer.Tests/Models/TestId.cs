using System;
using Brickweave.Domain;

namespace Brickweave.EventStore.SqlServer.Tests.Models
{
    public class TestId : Id<Guid>
    {
        public TestId(Guid value) : base(value)
        {
        }

        public static TestId NewId()
        {
            return new TestId(Guid.NewGuid());
        }
    }
}