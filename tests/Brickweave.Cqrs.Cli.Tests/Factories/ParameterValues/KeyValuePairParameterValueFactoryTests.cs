using System;
using System.Collections.Generic;
using System.Linq;
using Brickweave.Cqrs.Cli.Factories.ParameterValues;
using Brickweave.Cqrs.Cli.Models;
using Brickweave.Cqrs.Cli.Tests.Models;
using FluentAssertions;
using Xunit;

namespace Brickweave.Cqrs.Cli.Tests.Factories.ParameterValues
{
    public class KeyValuePairParameterValueFactoryTests
    {
        [Fact]
        public void Qualifies_WhenTypeIsKeyValuePair_ReturnsTrue()
        {
            var factory = new KeyValuePairParameterValueFactory(
                Enumerable.Empty<ISingleParameterValueFactory>());

            var result = factory.Qualifies(typeof(KeyValuePair<string, object>));

            result.Should().BeTrue();
        }

        [Fact]
        public void Qualifies_WhenTypeIsNotKeyValuePair_ReturnsFalse()
        {
            var factory = new KeyValuePairParameterValueFactory(
                Enumerable.Empty<ISingleParameterValueFactory>());

            var result = factory.Qualifies(typeof(string));

            result.Should().BeFalse();
        }

        [Fact]
        public void Create_WhenFormattedStringIsPassed_ReturnsKeyValuePair()
        {
            var factory = new KeyValuePairParameterValueFactory(
                new ISingleParameterValueFactory[] { new BasicParameterValueFactory() });

            var result = factory.Create(typeof(KeyValuePair<string, string>),
                new ExecutableParameterInfo("attributes", "key[=value]"));

            result.Should().BeOfType<KeyValuePair<string, string>>();
            result.As<KeyValuePair<string, string>>().Key.Should().Be("key");
            result.As<KeyValuePair<string, string>>().Value.Should().Be("value");
        }

        [Fact]
        public void Create_WhenValueIsIdModel_ReturnsKeyValuePairWithIdValue()
        {
            var factory = new KeyValuePairParameterValueFactory(
                new ISingleParameterValueFactory[] 
                {
                    new BasicParameterValueFactory(),
                    new WrappedGuidParameterValueFactory()
                });

            var result = factory.Create(typeof(KeyValuePair<string, BarId>),
                new ExecutableParameterInfo("attributes", "key[=B4E2CFB4-69AF-4E10-9903-5B80FA93BB42]"));

            result.Should().BeOfType<KeyValuePair<string, BarId>>();
            result.As<KeyValuePair<string, BarId>>().Key.Should().Be("key");
            result.As<KeyValuePair<string, BarId>>().Value.Should().Be(new BarId(new Guid("B4E2CFB4-69AF-4E10-9903-5B80FA93BB42")));
        }

        [Fact]
        public void Create_WhenValueIsInvalid_Throws()
        {
            var factory = new KeyValuePairParameterValueFactory(
                new ISingleParameterValueFactory[] { new BasicParameterValueFactory() });

            var exception = Record.Exception(() => factory.Create(typeof(KeyValuePair<string, string>),
                new ExecutableParameterInfo("attributes", "key[=a[=b]")));

            exception.Should().BeOfType<InvalidOperationException>();
        }
    }
}
