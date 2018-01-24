using System;
using Brickweave.Cqrs.Cli.Factories;
using Brickweave.Cqrs.Cli.Models;
using FluentAssertions;
using Xunit;

namespace Brickweave.Cqrs.Cli.Tests.Factories
{
    public class BasicParameterValueFactoryTests
    {
        [Theory]
        [InlineData(typeof(short))]
        [InlineData(typeof(short?))]
        [InlineData(typeof(int))]
        [InlineData(typeof(int?))]
        [InlineData(typeof(long))]
        [InlineData(typeof(long?))]
        [InlineData(typeof(ulong))]
        [InlineData(typeof(ulong?))]
        [InlineData(typeof(decimal))]
        [InlineData(typeof(decimal?))]
        [InlineData(typeof(float))]
        [InlineData(typeof(float?))]
        [InlineData(typeof(double))]
        [InlineData(typeof(double?))]
        [InlineData(typeof(char))]
        [InlineData(typeof(char?))]
        [InlineData(typeof(bool))]
        [InlineData(typeof(bool?))]
        [InlineData(typeof(string))]
        public void Qualifies_WhenSupportedTypeIsPassed_ReturnsTrue(Type type)
        {
            var factory = new BasicParameterValueFactory();

            var result = factory.Qualifies(type);

            result.Should().BeTrue();
        }

        [Fact]
        public void Qualifies_WhenUnSupportedTypeIsPassed_ReturnsFalse()
        {
            var factory = new BasicParameterValueFactory();

            var result = factory.Qualifies(GetType());

            result.Should().BeFalse();
        }

        [Fact]
        public void Create_WhenTargetTypeIsStringAndValueIsString_ReturnsStringValue()
        {
            var factory = new BasicParameterValueFactory();

            var result = factory.Create(typeof(string), new ExecutableParameterInfo("value", "foo"));

            result.Should().Be("foo");
        }

        [Fact]
        public void Create_WhenTargetTypeIsIntegerAndValueIsString_ReturnsIntegerValue()
        {
            var factory = new BasicParameterValueFactory();

            var result = factory.Create(typeof(int), new ExecutableParameterInfo("value", "123"));

            result.Should().Be(123);
        }
    }
}
