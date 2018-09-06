using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Brickweave.Cqrs.Cli.Exceptions;
using Brickweave.Cqrs.Cli.Factories;
using Brickweave.Cqrs.Cli.Factories.ParameterValues;
using Brickweave.Cqrs.Cli.Models;
using Brickweave.Cqrs.Cli.Tests.Models;
using FluentAssertions;
using Xunit;

namespace Brickweave.Cqrs.Cli.Tests.Factories
{
    public class ExecutableFactoryTests
    {
        [Fact]
        public void Exists_WhenExists_ReturnsTrue()
        {
            var factory = new ExecutableFactory(
                new[] { new BasicParameterValueFactory() },
                new[] { typeof(CreateFoo) });

            var result = factory.Exists("CreateFoo");

            result.Should().BeTrue();
        }

        [Fact]
        public void Exists_WhenNotExists_ReturnsFalse()
        {
            var factory = new ExecutableFactory(
                new[] { new BasicParameterValueFactory() },
                new[] { typeof(CreateFoo) });

            var result = factory.Exists("CreateBar");

            result.Should().BeFalse();
        }

        [Fact]
        public void Create_WhenCommandAssemblyIsRegistered_CreatesCommand()
        {
            var factory = new ExecutableFactory(
                new IParameterValueFactory[] { new BasicParameterValueFactory(), new DateTimeParameterValueFactory(CultureInfo.InvariantCulture) },
                new [] { typeof(CreateFoo) });

            var created = new DateTime(2017, 1, 1, 14, 0, 0);
            var result = factory.Create(
                typeof(CreateFoo),
                new ExecutableParameterInfo("bar", "something"),
                new ExecutableParameterInfo("id", "12345"),
                new ExecutableParameterInfo("datecreated", created.ToString(CultureInfo.InvariantCulture)));

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

            var result = factory.Create(typeof(CreateFoo));

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

            var exception = Assert.Throws<ExecutableNotFoundException>(() => factory.Create(new ExecutableInfo(
                "CreateFooBar", Enumerable.Empty<ExecutableParameterInfo>())));

            exception.TypeShortName.Should().Be("CreateFooBar");
        }

        [Fact]
        public void Create_WhenExecutableTypeConstructorDoesNotMatchPassedParameters_Throws()
        {
            var factory = new ExecutableFactory(
                new[] { new BasicParameterValueFactory() },
                new[] { typeof(CreateFoo) });
            
            var parameters = new List<ExecutableParameterInfo>
            {
                new ExecutableParameterInfo("id", "1"),
                new ExecutableParameterInfo("dateCreated", DateTime.Now.ToString(CultureInfo.InvariantCulture)),
                new ExecutableParameterInfo("foo", "bar")
            };
            
            var exception = Assert.Throws<ConstructorNotFoundException>(() => 
                factory.Create(new ExecutableInfo("CreateFoo", parameters)));

            exception.Type.Should().Be(typeof(CreateFoo));
            exception.Parameters.Should().BeEquivalentTo(parameters.Select(p => p.Name).ToList());
        }

        [Fact]
        public void Create_WhenNoParameterFactoryQualifiesForCommandConstructorParameterType_Throws()
        {
            var factory = new ExecutableFactory(
                Enumerable.Empty<IParameterValueFactory>(),
                new[] { typeof(CreateFoo) });

            var exception = Assert.Throws<NoQualifyingParameterValueFactoryException>(() => factory.Create(new ExecutableInfo("CreateFoo", 
                new [] { new ExecutableParameterInfo("id", "1") })));
            
            exception.TypeShortName.Should().Be("Int32");
        }

        [Fact]
        public void Create_WhenParameterValueIsNotProvidedAndParameterIsNullable_ReturnsCommandWithNullPropertyValue()
        {
            var name = "bar1";

            var factory = new ExecutableFactory(
                new IParameterValueFactory[] 
                {
                    new BasicParameterValueFactory(),
                    new WrappedGuidParameterValueFactory()
                },
                new[] { typeof(GetBar) });

            var executableInfo = new ExecutableInfo("GetBar",
                new[] { new ExecutableParameterInfo("name", name) });

            var result = factory.Create(executableInfo) as GetBar;

            result.Should().NotBeNull();
            result.Id.Should().BeNull();
            result.Name.Should().Be(name);
        }
    }
}
