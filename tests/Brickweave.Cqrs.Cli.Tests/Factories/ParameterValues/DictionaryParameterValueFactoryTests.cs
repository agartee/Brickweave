using System;
using System.Collections.Generic;
using System.Linq;
using Brickweave.Cqrs.Cli.Factories.ParameterValues;
using Brickweave.Cqrs.Cli.Models;
using FluentAssertions;
using Xunit;

namespace Brickweave.Cqrs.Cli.Tests.Factories.ParameterValues
{
    public class DictionaryParameterValueFactoryTests
    {
        [Theory]
        [InlineData(typeof(IDictionary<string, string>))]
        [InlineData(typeof(Dictionary<string, string>))]
        public void Qualifies_WhenSupportedTypeIsPassed_ReturnsTrue(Type type)
        {
            var factory = new DictionaryParameterValueFactory(
                new KeyValuePairParameterValueFactory(Enumerable.Empty<ISingleParameterValueFactory>()));

            var result = factory.Qualifies(type);

            result.Should().BeTrue();
        }

        [Fact]
        public void Qualifies_WhenUnSupportedTypeIsPassed_ReturnsFalse()
        {
            var factory = new DictionaryParameterValueFactory(
                new KeyValuePairParameterValueFactory(Enumerable.Empty<ISingleParameterValueFactory>()));

            var result = factory.Qualifies(typeof(string));

            result.Should().BeFalse();
        }

        [Fact]
        public void Create_WithSingleKeyValuePair_ReturnsDictionary()
        {
            var factory = new DictionaryParameterValueFactory(
                new KeyValuePairParameterValueFactory(new[] { new BasicParameterValueFactory() }));

            var result = factory.Create(typeof(Dictionary<string, string>),
                new ExecutableParameterInfo("attributes", "key[=value]"));

            result.Should().BeOfType<Dictionary<string, string>>();
            result.As<Dictionary<string, string>>()["key"].Should().Be("value");
        }

        [Fact]
        public void Create_WithSingleKeyValuePairWhereKeyContainsSpaces_ReturnsDictionary()
        {
            var factory = new DictionaryParameterValueFactory(
                new KeyValuePairParameterValueFactory(new[] { new BasicParameterValueFactory() }));

            var result = factory.Create(typeof(Dictionary<string, string>),
                new ExecutableParameterInfo("attributes", "\"my key\"[=value]"));

            result.Should().BeOfType<Dictionary<string, string>>();
            result.As<Dictionary<string, string>>()["my key"].Should().Be("value");
        }

        [Fact]
        public void Create_WithMultipleValuesForSameKeyAndIEnumerableGenericValue_ReturnsDictionary()
        {
            var factory = new DictionaryParameterValueFactory(
                new KeyValuePairParameterValueFactory(new[] 
                {
                    new BasicParameterValueFactory() 
                }));

            var result = factory.Create(typeof(IDictionary<string, IEnumerable<string>>),
                new ExecutableParameterInfo("attributes", "key[=value1]", "key[=value2]"));

            result.Should().BeOfType<Dictionary<string, IEnumerable<string>>>();
            result.As<IDictionary<string, IEnumerable<string>>>()["key"].Should().Contain("value1", "value2");
        }

        [Fact]
        public void Create_WithMultipleValuesForSameKeyAndIListGenericValue_ReturnsDictionary()
        {
            var factory = new DictionaryParameterValueFactory(
                new KeyValuePairParameterValueFactory(new[]
                {
                    new BasicParameterValueFactory()
                }));

            var result = factory.Create(typeof(IDictionary<string, IList<string>>),
                new ExecutableParameterInfo("attributes", "key[=value1]", "key[=value2]"));

            result.Should().BeOfType<Dictionary<string, IList<string>>>();
            result.As<IDictionary<string, IList<string>>>()["key"].Should().Contain("value1", "value2");
        }

        [Fact]
        public void Create_WithMultipleValuesForSameKeyAndListGenericValue_ReturnsDictionary()
        {
            var factory = new DictionaryParameterValueFactory(
                new KeyValuePairParameterValueFactory(new[]
                {
                    new BasicParameterValueFactory()
                }));

            var result = factory.Create(typeof(IDictionary<string, List<string>>),
                new ExecutableParameterInfo("attributes", "key[=value1]", "key[=value2]"));

            result.Should().BeOfType<Dictionary<string, List<string>>>();
            result.As<IDictionary<string, List<string>>>()["key"].Should().Contain("value1", "value2");
        }
    }
}
