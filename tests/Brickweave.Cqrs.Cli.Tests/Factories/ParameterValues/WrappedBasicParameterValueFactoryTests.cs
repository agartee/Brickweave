using Brickweave.Cqrs.Cli.Factories.ParameterValues;
using Brickweave.Cqrs.Cli.Models;
using Brickweave.Cqrs.Cli.Tests.Models;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Brickweave.Cqrs.Cli.Tests.Factories.ParameterValues
{
    public class WrappedBasicParameterValueFactoryTests
    {
        [Fact]
        public void Qualifies_WhenTypeIsWrappedBasicType_ReturnsTrue()
        {
            var factory = new WrappedBasicParameterValueFactory();

            var result = factory.Qualifies(typeof(FooId));
            
            result.Should().BeTrue();
        }

        [Fact]
        public void Qualifies_WhenTypeIsBasicPrimitiveType_ReturnsFalse()
        {
            var factory = new WrappedBasicParameterValueFactory();

            var result = factory.Qualifies(typeof(string));

            result.Should().BeFalse();
        }


        [Fact]
        public void Qualifies_WhenTypeIsDictionary_ReturnsFalse()
        {
            var factory = new WrappedBasicParameterValueFactory();

            var result = factory.Qualifies(typeof(Dictionary<string, object>));

            result.Should().BeFalse();
        }

        [Fact]
        public void Create_WhenTypeIsWrappedBasicType_ReturnsWrapperInstance()
        {
            var factory = new WrappedBasicParameterValueFactory();

            var result = factory.Create(typeof(FooId), new ExecutableParameterInfo("value", "1"));

            result.Should().BeOfType<FooId>();
            result.As<FooId>().Value.Should().Be("1");
        }

        [Fact]
        public void Create_WhenParameterValueIsNull_ReturnsNull()
        {
            var factory = new WrappedBasicParameterValueFactory();

            var result = factory.Create(typeof(FooId), null);

            result.Should().BeNull();
        }
    }
}
