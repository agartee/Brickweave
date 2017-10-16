using System;
using System.Collections.Generic;
using System.Globalization;
using Brickweave.Cqrs.Cli.Exceptions;
using Brickweave.Cqrs.Cli.Factories;
using Brickweave.Cqrs.Cli.Models;
using Brickweave.Cqrs.Cli.Tests.Models;
using FluentAssertions;
using Xunit;

namespace Brickweave.Cqrs.Cli.Tests.Factories
{
    public class ExecutableFactoryTests
    {
        [Fact]
        public void Create_WhenCommandAssemblyIsRegistered_CreatesCommand()
        {
            var factory = new ExecutableFactory(
                new [] { new BasicParameterValueFactory() },
                new [] { typeof(CreateFoo) });

            var created = new DateTime(2017, 1, 1, 14, 0, 0);
            var result = factory.Create(typeof(CreateFoo), new Dictionary<string, string>
            {
                ["bar"] = "something",
                ["id"] = "12345",
                ["datecreated"] = created.ToString(CultureInfo.InvariantCulture)
            });

            result.Should().NotBeNull();
            result.Should().BeOfType<CreateFoo>();
            result.As<CreateFoo>().Id.Should().Be(12345);
            result.As<CreateFoo>().Bar.Should().Be("something");
            result.As<CreateFoo>().DateCreated.Should().Be(created);
        }

        [Fact]
        public void Create_WhenMissingParameterThatHasDefaultValue_CreatesCommandWithDefaultValue()
        {
            var factory = new ExecutableFactory(
                new[] { new BasicParameterValueFactory() },
                new[] { typeof(CreateFoo) });

            var result = factory.Create(typeof(CreateFoo), new Dictionary<string, string>());

            result.Should().NotBeNull();
            result.Should().BeOfType<CreateFoo>();
            result.As<CreateFoo>().Bar.Should().Be("bar");
        }

        [Fact]
        public void Create_WhenExecutableTypeNameIsNotInRegisteredAssemblies_Throws() 
        {
            var factory = new ExecutableFactory(
                new[] { new BasicParameterValueFactory() },
                new[] { typeof(CreateFoo) });

            var exception = Assert.Throws<TypeNotFoundException>(() => factory.Create(new ExecutableInfo(
                "CreateFooBar", new Dictionary<string, string>())));

            exception.Should().NotBeNull();
            exception.TypeShortName.Should().Be("CreateFooBar");
        }
    }
}
