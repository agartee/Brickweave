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
    public class EnumerableParameterValueFactoryTests
    {
        [Theory]
        [InlineData(typeof(IEnumerable<string>))]
        [InlineData(typeof(IEnumerable<FooId>))]
        public void Qualifies_WhenSupportedTypeIsPassed_ReturnsTrue(Type type)
        {
            var factory = new EnumerableParameterValueFactory(
                new IParameterValueFactory[] {});

            var result = factory.Qualifies(type);

            result.Should().BeTrue();
        }

        [Fact]
        public void Qualifies_WhenUnSupportedTypeIsPassed_ReturnsFalse()
        {
            var factory = new EnumerableParameterValueFactory(
                new IParameterValueFactory[] { });

            var result = factory.Qualifies(typeof(List<string>));

            result.Should().BeFalse();
        }

        [Fact]
        public void Create_WhenNoParameterFactoryQualifiesForListGenericArg_Throws()
        {
            var factory = new EnumerableParameterValueFactory(
                Enumerable.Empty<IParameterValueFactory>());

            var exception = Assert.Throws<NoQualifyingParameterValueFactoryException>(() => 
                factory.Create(typeof(IEnumerable<string>), new ExecutableParameterInfo("foo")));
            
            exception.TypeShortName.Should().Be("String");
        }

        [Fact]
        public void Create_WhenTargetTypeIsStringIEnumerableAndContainsNoValues_ReturnsEmptyStringArray()
        {
            var factory = new EnumerableParameterValueFactory(
                new IParameterValueFactory[] { new BasicParameterValueFactory() });

            var result = factory.Create(typeof(IEnumerable<string>), new ExecutableParameterInfo("foo"));

            result.Should().BeOfType<string[]>();
            result.As<string[]>().Should().BeEmpty();
        }
        
        [Fact]
        public void Create_WhenTargetTypeIsStringListAndContainsTwoValues_ReturnsStringList()
        {
            var factory = new EnumerableParameterValueFactory(
                new IParameterValueFactory[] { new BasicParameterValueFactory() });

            var result = factory.Create(typeof(IEnumerable<string>), new ExecutableParameterInfo("value", "foo", "bar"));

            result.Should().BeOfType<string[]>();
            result.ShouldBeEquivalentTo(new [] { "foo", "bar" });
        }
    }
}