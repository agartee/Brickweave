using Brickweave.Cqrs.Cli.Factories;
using Brickweave.Cqrs.Cli.Models;
using Brickweave.Cqrs.Cli.Tests.Models;
using FluentAssertions;
using Xunit;

namespace Brickweave.Cqrs.Cli.Tests.Factories
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
        public void Qualifies_WhenTypeIsNotWrappedBasicType_ReturnsFalse()
        {
            var factory = new WrappedBasicParameterValueFactory();

            var result = factory.Qualifies(typeof(string));

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
    }
}
