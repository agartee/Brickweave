using System;
using Brickweave.Cqrs.Cli.Extensions;
using FluentAssertions;
using Xunit;

namespace Brickweave.Domain.Tests.Extensions
{
    public class ObjectExtensionsTests
    {
        [Fact]
        public void AutoCorrectType_WhenValueIsGuidAsString_ReturnsGuid()
        {
            var value = Guid.NewGuid().ToString();

            var result = value.AutoCorrectType();

            result.Should().BeOfType<Guid>();
            result.ToString().Should().Be(value);
        }

        [Fact]
        public void AutoCorrectType_WhenValueIsInt_ReturnsInt()
        {
            var value = "1";

            var result = value.AutoCorrectType();

            result.Should().BeOfType<long>();
            result.Should().Be(1L);
        }

        [Fact]
        public void AutoCorrectType_WhenValueIsString_ReturnsString()
        {
            var value = "hi.";

            var result = value.AutoCorrectType();

            result.Should().BeOfType<string>();
            result.Should().Be(value);
        }
    }
}
