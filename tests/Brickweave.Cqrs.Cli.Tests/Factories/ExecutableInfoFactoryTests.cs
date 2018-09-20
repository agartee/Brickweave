using System.Linq;
using Brickweave.Cqrs.Cli.Factories;
using Brickweave.Cqrs.Cli.Tests.Models;
using FluentAssertions;
using Xunit;

namespace Brickweave.Cqrs.Cli.Tests.Factories
{
    public class ExecutableInfoFactoryTests
    {
        [Fact]
        public void Create_ReturnsExecutableInfo()
        {
            var factory = new ExecutableInfoFactory();

            var result = factory.Create(new[] { "foo", "create", "--id", "12345" });

            result.Name.Should().Be("CreateFoo");
            result.Parameters.Should().HaveCount(1);
            result.Parameters.First(p => p.Name.Equals("id"))
                .Values.First().Should().Be("12345");
        }

        [Fact]
        public void Create_WithDashDashParameterThatHasNoValue_DefaultsValueToTrue()
        {
            var factory = new ExecutableInfoFactory();

            var result = factory.Create(new[] { "foo", "create", "--iscool", "--id", "12345" });

            result.Name.Should().Be("CreateFoo");
            result.Parameters.Should().HaveCount(2);
            result.Parameters.First(p => p.Name.Equals("iscool"))
                .Values.First().Should().Be("true");
            result.Parameters.First(p => p.Name.Equals("id"))
                .Values.First().Should().Be("12345");
        }

        [Fact]
        public void Create_WithArgThatHasMultipleParameterValuesInSeparateArgs_ReturnsExecutableInfoWithMultipleParameterValues()
        {
            var factory = new ExecutableInfoFactory();

            var result = factory.Create(new[] { "foo", "create", "--ids", "1", "2" });

            result.Name.Should().Be("CreateFoo");
            result.Parameters.Should().HaveCount(1);

            result.Parameters.First(p => p.Name.Equals("ids"))
                .Values.Should().BeEquivalentTo("1", "2");
        }

        [Fact]
        public void Create_WithArgThatHasMultipleMultiParameterValuesInSeparateArgs_ReturnsExecutableInfoWithMultipleParameterValues()
        {
            var factory = new ExecutableInfoFactory();

            var result = factory.Create(new[] { "foo", "create", "--ids", "1", "2", "--other" , "a", "b" });

            result.Name.Should().Be("CreateFoo");
            result.Parameters.Should().HaveCount(2);
            result.Parameters.First(p => p.Name.Equals("ids"))
                .Values.Should().BeEquivalentTo("1", "2");
            result.Parameters.First(p => p.Name.Equals("other"))
                .Values.Should().BeEquivalentTo("a", "b");
        }

        [Fact]
        public void Create_WithNoDashDashParameters_ReturnsExecutableInfo()
        {
            var factory = new ExecutableInfoFactory();

            var result = factory.Create(new[] { "foo", "list" });

            result.Name.Should().Be("ListFoo");
            result.Parameters.Should().HaveCount(0);
        }
        
        [Fact]
        public void Create_WhenExecutableIsRegistered_ReturnsExecutableInfoFromRegistration()
        {
            var factory = new ExecutableInfoFactory(
                new ExecutableRegistration<ListFoos>("list", "foo"));

            var result = factory.Create(new[] { "foo", "list" });

            result.Name.Should().Be(nameof(ListFoos));
            result.Parameters.Should().HaveCount(0);
        }

        [Fact]
        public void Create_WhenParametersContainKeyValuePair_ReturnsExecutableInfo()
        {
            var factory = new ExecutableInfoFactory(
                new ExecutableRegistration<ListBars>("list", "bar"));

            var result = factory.Create(new[] { "bar", "list", "--attributes", "key[=this is a value]" });

            result.Name.Should().Be(nameof(ListBars));
            result.Parameters.Should().HaveCount(1);
        }
    }
}
