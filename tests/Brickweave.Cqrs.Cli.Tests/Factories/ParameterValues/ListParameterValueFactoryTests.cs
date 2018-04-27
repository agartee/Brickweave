using System;
using System.Collections.Generic;
using System.Linq;
using Brickweave.Cqrs.Cli.Exceptions;
using Brickweave.Cqrs.Cli.Factories.ParameterValues;
using Brickweave.Cqrs.Cli.Models;
using Brickweave.Cqrs.Cli.Tests.Models;
using FluentAssertions;
using Xunit;

namespace Brickweave.Cqrs.Cli.Tests.Factories.ParameterValues
{
    public class ListParameterValueFactoryTests
    {
        [Theory]
        [InlineData(typeof(List<string>))]
        [InlineData(typeof(List<FooId>))]
        [InlineData(typeof(IList<string>))]
        [InlineData(typeof(IList<FooId>))]
        public void Qualifies_WhenSupportedTypeIsPassed_ReturnsTrue(Type type)
        {
            var factory = new ListParameterValueFactory(
                new IParameterValueFactory[] { });

            var result = factory.Qualifies(type);

            result.Should().BeTrue();
        }

        [Fact]
        public void Qualifies_WhenUnSupportedTypeIsPassed_ReturnsFalse()
        {
            var factory = new ListParameterValueFactory(
                new IParameterValueFactory[] { });

            var result = factory.Qualifies(typeof(IEnumerable<string>));

            result.Should().BeFalse();
        }

        [Fact]
        public void Create_WhenNoParameterFactoryQualifiesForListGenericArg_Throws()
        {
            var factory = new ListParameterValueFactory(
                Enumerable.Empty<IParameterValueFactory>());

            var exception = Assert.Throws<NoQualifyingParameterValueFactoryException>(() =>
                factory.Create(typeof(List<string>), new ExecutableParameterInfo("foo")));

            exception.TypeShortName.Should().Be("String");
        }

        [Fact]
        public void Create_WhenTargetTypeIsStringIEnumerableAndContainsNoValues_ReturnsEmptyStringList()
        {
            var factory = new ListParameterValueFactory(
                new IParameterValueFactory[] { new BasicParameterValueFactory() });

            var result = factory.Create(typeof(List<string>), new ExecutableParameterInfo("foo"));

            result.Should().BeOfType<List<string>>();
            result.As<List<string>>().Should().BeEmpty();
        }

        [Fact]
        public void Create_WhenTargetTypeIsStringListAndContainsTwoValues_ReturnsStringList()
        {
            var factory = new ListParameterValueFactory(
                new IParameterValueFactory[] { new BasicParameterValueFactory() });

            var result = factory.Create(typeof(IEnumerable<string>), new ExecutableParameterInfo("value", "foo", "bar"));

            result.Should().BeOfType<List<string>>();
            result.Should().BeEquivalentTo(new List<string> { "foo", "bar" });
        }
    }
}