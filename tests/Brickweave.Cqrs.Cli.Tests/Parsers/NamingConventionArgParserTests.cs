using Brickweave.Cqrs.Cli.Parsers;
using FluentAssertions;
using Xunit;

namespace Brickweave.Cqrs.Cli.Tests.Parsers
{
    public class NamingConventionArgParserTests
    {
        [Fact]
        public void Parse_ReturnsExecutableInfo()
        {
            var parser = new NamingConventionArgParser();

            var result = parser.Parse(new[] { "foo", "create", "--id", "12345" });

            result.Name.Should().Be("CreateFoo");
            result.Parameters.Should().HaveCount(1);
            result.Parameters["id"].Should().Be("12345");
        }

        [Fact]
        public void Parse_WhenHasParameterWithNoValue_DefaultsValueToTrue()
        {
            var parser = new NamingConventionArgParser();

            var result = parser.Parse(new[] { "foo", "create", "--iscool", "--id", "12345" });

            result.Name.Should().Be("CreateFoo");
            result.Parameters.Should().HaveCount(1);
            result.Parameters["iscool"].Should().Be("true");
        }
    }
}
