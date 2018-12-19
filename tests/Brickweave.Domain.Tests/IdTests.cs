using System;
using FluentAssertions;
using Xunit;

namespace Brickweave.Domain.Tests
{
    public class IdTests
    {
        [Fact]
        public void Equals_WhenSameTypeAndUnderlyingValue_ReturnsTrue()
        {
            var backingValue = Guid.NewGuid();

            var id1 = new TestId(backingValue);
            var id2 = new TestId(backingValue);

            id1.Should().Be(id2);
        }

        [Fact]
        public void Equals_WhenSameTypeButDifferentUnderlyingValue_ReturnsFalse()
        {
            var id1 = TestId.NewId();
            var id2 = TestId.NewId();

            id1.Should().NotBeSameAs(id2);
        }

        private class TestId : Id<Guid>
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
}
