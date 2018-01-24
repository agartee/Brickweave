using Brickweave.Cqrs.Cli.Models;
using FluentAssertions;
using Xunit;

namespace Brickweave.Cqrs.Cli.Tests.Models
{
    public class ExecutableParameterInfoTests
    {
        [Fact]
        public void Equals_WhenEquivalent_ReturnsTrue()
        {
            var info = new ExecutableParameterInfo("name", "one", "two");

            var result = info.Equals(new ExecutableParameterInfo("name", "one", "two"));

            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_WhenEquivalentButValuesInDifferentOrder_ReturnsTrue()
        {
            var info = new ExecutableParameterInfo("name", "one", "two");

            var result = info.Equals(new ExecutableParameterInfo("name", "two", "one"));

            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_WhenNotEquivalent_ReturnsFalse()
        {
            var info = new ExecutableParameterInfo("name", "one", "two");

            var result = info.Equals(new ExecutableParameterInfo("name", "one", "three"));

            result.Should().BeFalse();
        }

        [Fact]
        public void GetHashCode_WhenEquivalent_AreEqual()
        {
            var info1 = new ExecutableParameterInfo("name", "one", "two");
            var info2 = new ExecutableParameterInfo("name", "one", "two");

            info1.GetHashCode().Should().Be(info2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_WhenEquivalentButValuesInDifferentOrder_AreEqual()
        {
            var info1 = new ExecutableParameterInfo("name", "one", "two");
            var info2 = new ExecutableParameterInfo("name", "two", "one");

            info1.GetHashCode().Should().Be(info2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_WhenNotEquivalent_AreNotEqual()
        {
            var info1 = new ExecutableParameterInfo("name", "one", "two");
            var info2 = new ExecutableParameterInfo("name", "one", "three");

            info1.GetHashCode().Should().NotBe(info2.GetHashCode());
        }
    }
}