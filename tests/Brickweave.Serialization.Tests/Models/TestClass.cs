using System;

namespace Brickweave.Serialization.Tests.Models
{
    public class TestClass
    {
        public TestClass(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
