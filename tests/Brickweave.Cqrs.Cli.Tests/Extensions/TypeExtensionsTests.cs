using Brickweave.Cqrs.Cli.Extensions;
using Brickweave.Cqrs.Cli.Tests.Models;
using FluentAssertions;
using Xunit;

namespace Brickweave.Cqrs.Cli.Tests.Extensions
{
    public class TypeExtensionsTests
    {
        [Fact]
        public void HasConstructorWithParameters_WhenTypeHasConstructorWithParameters_ReturnsTrue()
        {
            var type = typeof(CreateFoo);

            var result = type.HasConstructorWithParameters();

            result.Should().BeTrue();
        }

        [Fact]
        public void HasConstructorWithParameters_WhenTypeDoesNotHaveConstructorWithParameters_ReturnsFalse()
        {
            var type = typeof(CreateQux);

            var result = type.HasConstructorWithParameters();

            result.Should().BeFalse();
        }
    }
}
