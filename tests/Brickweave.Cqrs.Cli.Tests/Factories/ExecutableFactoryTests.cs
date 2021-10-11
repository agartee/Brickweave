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
        public void Create_WhenCommandAssemblyIsRegistered_CreatesCommandViaConstructor()
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
        public void Create_WhenCommandAssemblyIsRegistered_CreatesCommandViaProperties()
        {
            var factory = new ExecutableFactory(
                new IParameterValueFactory[] { new BasicParameterValueFactory(), new DateTimeParameterValueFactory(CultureInfo.InvariantCulture) },
                new[] { typeof(CreateQux) });

            var created = new DateTime(2017, 1, 1, 14, 0, 0);
            var result = factory.Create(
                typeof(CreateQux),
                new ExecutableParameterInfo("bar", "something"),
                new ExecutableParameterInfo("id", "12345"),
                new ExecutableParameterInfo("datecreated", created.ToString(CultureInfo.InvariantCulture)));

            result.Should().NotBeNull();
            result.Should().BeOfType<CreateQux>();
            result.As<CreateQux>().Id.Should().Be(12345);
            result.As<CreateQux>().Bar.Should().Be("something");
            result.As<CreateQux>().DateCreated.Should().Be(created);
        }

        [Fact]
        public void Create_WhenMissingParameterThatHasDefaultValue_CreatesCommandWithDefaultValueViaConstructor()
        {
            var factory = new ExecutableFactory(
                new[] { new BasicParameterValueFactory() },
                new[] { typeof(CreateFoo) });

            var result = factory.Create(typeof(CreateFoo));

            result.Should().NotBeNull();
            result.Should().BeOfType<CreateFoo>();
            result.As<CreateFoo>().Id.Should().Be(0);
            result.As<CreateFoo>().Bar.Should().Be("bar");
            result.As<CreateFoo>().DateCreated.Should().Be(DateTime.MinValue);
        }

        [Fact]
        public void Create_WhenMissingParameterThatHasDefaultValue_CreatesCommandWithDefaultValueViaProperties()
        {
            var factory = new ExecutableFactory(
                new[] { new BasicParameterValueFactory() },
                new[] { typeof(CreateQux) });

            var result = factory.Create(typeof(CreateQux));

            result.Should().NotBeNull();
            result.Should().BeOfType<CreateQux>();
            result.As<CreateQux>().Id.Should().Be(0);
            result.As<CreateQux>().Bar.Should().Be("bar");
            result.As<CreateQux>().DateCreated.Should().Be(DateTime.MinValue);
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
                new ExecutableParameterInfo("foo", "foo does not exist in the class constructor")
            };

            var exception = Assert.Throws<ConstructorNotFoundException>(() =>
                factory.Create(new ExecutableInfo("CreateFoo", parameters)));

            exception.Type.Should().Be(typeof(CreateFoo));
            exception.Parameters.Should().BeEquivalentTo(parameters.Select(p => p.Name).ToList());
        }

        [Fact]
        public void Create_WhenExecutableTypePropertiesDoNotMatchPassedParameters_Throws()
        {
            var factory = new ExecutableFactory(
                new[] { new BasicParameterValueFactory() },
                new[] { typeof(CreateQux) });

            var parameters = new List<ExecutableParameterInfo>
            {
                new ExecutableParameterInfo("id", "1"),
                new ExecutableParameterInfo("dateCreated", DateTime.Now.ToString(CultureInfo.InvariantCulture)),
                new ExecutableParameterInfo("qux", "qux does not exist in the class as a property")
            };

            var exception = Assert.Throws<PropertyNotFoundException>(() =>
                factory.Create(new ExecutableInfo("CreateQux", parameters)));

            exception.Type.Should().Be(typeof(CreateQux));
            exception.Parameter.Should().BeEquivalentTo("qux");
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
        public void Create_WhenParameterValueIsNotProvidedAndParameterIsNullable_ReturnsCommandWithNullPropertyValueViaConstructor()
        {
            var factory = new ExecutableFactory(
                new IParameterValueFactory[] 
                {
                    new BasicParameterValueFactory(),
                    new WrappedGuidParameterValueFactory()
                },
                new[] { typeof(GetBar) });

            var executableInfo = new ExecutableInfo("GetBar",
                new ExecutableParameterInfo[] { });

            var result = factory.Create(executableInfo) as GetBar;

            result.Should().NotBeNull();
            result.Name.Should().BeNull();
        }

        [Fact]
        public void Create_WhenParameterValueIsNotProvidedAndParameterIsNullable_ReturnsCommandWithNullPropertyValueViaProperties()
        {
            var factory = new ExecutableFactory(
                new IParameterValueFactory[]
                {
                    new BasicParameterValueFactory(),
                    new WrappedGuidParameterValueFactory()
                },
                new[] { typeof(GetWaldo) });

            var executableInfo = new ExecutableInfo("GetWaldo",
                new ExecutableParameterInfo[] { });

            var result = factory.Create(executableInfo) as GetWaldo;

            result.Should().NotBeNull();
            result.Name.Should().BeNull();
        }
    }
}
