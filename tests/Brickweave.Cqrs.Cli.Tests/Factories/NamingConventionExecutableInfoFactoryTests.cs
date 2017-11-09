using Brickweave.Cqrs.Cli.Factories;
using FluentAssertions;
using Xunit;

namespace Brickweave.Cqrs.Cli.Tests.Factories
{
    public class NamingConventionExecutableInfoFactoryTests
    {
        [Fact]
        public void Parse_ReturnsExecutableInfo()
        {
            var factory = new NamingConventionExecutableInfoFactory();

            var result = factory.Create(new[] { "foo", "create", "--id", "12345" });

            result.Name.Should().Be("CreateFoo");
            result.Parameters.Should().HaveCount(1);
            result.Parameters["id"].Should().Be("12345");
        }

        [Fact]
        public void Parse_WhenHasParameterWithNoValue_DefaultsValueToTrue()
        {
            var factory = new NamingConventionExecutableInfoFactory();

            var result = factory.Create(new[] { "foo", "create", "--iscool", "--id", "12345" });

            result.Name.Should().Be("CreateFoo");
            result.Parameters.Should().HaveCount(1);
            result.Parameters["iscool"].Should().Be("true");
        }
    }
}
