using System;
using Brickweave.Cqrs.Cli.Extensions;
using FluentAssertions;
using Xunit;

namespace Brickweave.Cqrs.Cli.Tests.Extensions
{
    public class ObjectExtensionsTests
    {
        [Fact]
        public void CorrectType_WhenValueIsGuidAsString_ReturnsGuid()
        {
            var value = Guid.NewGuid().ToString();

            var result = value.InterpretType();

            result.Should().BeOfType<Guid>();
            result.ToString().Should().Be(value);
        }

        [Fact]
        public void CorrectType_WhenValueIsInt_ReturnsInt()
        {
            var value = "1";

            var result = value.InterpretType();
            
            result.Should().BeOfType<long>();
            result.Should().Be(1L);
        }

        [Fact]
        public void CorrectType_WhenValueIsString_ReturnsString()
        {
            var value = "hi.";

            var result = value.InterpretType();

            result.Should().BeOfType<string>();
            result.Should().Be(value);
        }
    }
}
