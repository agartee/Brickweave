using System.Linq;
using Brickweave.Cqrs.Cli.Extensions;
using Brickweave.Cqrs.Cli.Tests.Models;
using FluentAssertions;
using Xunit;

namespace Brickweave.Cqrs.Cli.Tests.Extensions
{
    public class ConstructorInfoExtensionsTests
    {
        [Fact]
        public void ContainsAllParameters_WhenConstuctorContainsAllParameterNames_ReturnsTrue()
        {
            var constructor = typeof(CreateFoo).GetConstructors().First();

            var result = constructor.ContainsAllParameters(new[] {"id", "dateCreated", "bar" });

            result.Should().BeTrue();
        }

        [Fact]
        public void ContainsAllParameters_WhenConstuctorContainsAllParameterNamesButAreOfMixedCase_ReturnsTrue()
        {
            var constructor = typeof(CreateFoo).GetConstructors().First();

            var result = constructor.ContainsAllParameters(new[] { "ID", "DateCreated", "Bar" });

            result.Should().BeTrue();
        }

        [Fact]
        public void ContainsAllParameters_WhenConstuctorDoesNotContainAllParameterNames_ReturnsFalse()
        {
            var constructor = typeof(CreateFoo).GetConstructors().First();

            var result = constructor.ContainsAllParameters(new[] { "id", "dateCreated", "foo" });

            result.Should().BeFalse();
        }

        [Fact]
        public void ContainsAllParameters_WhenConstuctorContainsAllNonNullableParameterNames_ReturnsTrue()
        {
            var constructor = typeof(CreateFoo).GetConstructors().First();

            var result = constructor.ContainsAllParameters(new[] { "id", "dateCreated" });

            result.Should().BeTrue();
        }
    }
}
