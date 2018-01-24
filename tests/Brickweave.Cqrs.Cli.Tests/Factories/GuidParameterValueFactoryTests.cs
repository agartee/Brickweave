using System;
using Brickweave.Cqrs.Cli.Factories;
using Brickweave.Cqrs.Cli.Models;
using Brickweave.Cqrs.Cli.Tests.Models;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Brickweave.Cqrs.Cli.Tests.Factories
{
    public class GuidParameterValueFactoryTests
    {
        private readonly ITestOutputHelper _output;

        public GuidParameterValueFactoryTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Qualifies_WhenTypeIsGuid_ReturnsTrue()
        {
            var factory = new GuidParameterValueFactory();

            var result = factory.Qualifies(typeof(Guid));

            result.Should().BeTrue();
        }

        [Fact]
        public void Qualifies_WhenTypeIsNotGuid_ReturnsFalse()
        {
            var factory = new GuidParameterValueFactory();

            var result = factory.Qualifies(typeof(FooId));

            result.Should().BeFalse();
        }

        [Fact]
        public void Create_WhenParameterIsString_ReturnsGuid()
        {
            var factory = new GuidParameterValueFactory();

            var result = factory.Create(typeof(Guid), new ExecutableParameterInfo("value", "{693B3DBF-2DF1-4EEE-AE36-7A1D070959B8}"));

            result.Should().Be(new Guid("{693B3DBF-2DF1-4EEE-AE36-7A1D070959B8}"));
        }

        [Fact]
        public void Create_WhenParameterValueIsNotString_Throws()
        {
            var factory = new GuidParameterValueFactory();

            var exception = Assert.Throws<FormatException>(() => factory.Create(typeof(Guid), new ExecutableParameterInfo("value", "1")));

            _output.WriteLine(exception.Message);
        }
    }
}