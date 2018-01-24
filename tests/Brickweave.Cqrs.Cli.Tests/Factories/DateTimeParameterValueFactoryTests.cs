using System;
using System.Globalization;
using Brickweave.Cqrs.Cli.Factories;
using Brickweave.Cqrs.Cli.Models;
using FluentAssertions;
using Xunit;

namespace Brickweave.Cqrs.Cli.Tests.Factories
{
    public class DateTimeParameterValueFactoryTests
    {
        [Fact]
        public void Qualifies_WhenTypeIsDateTime_ReturnsTrue()
        {
            var factory = new DateTimeParameterValueFactory(CultureInfo.InvariantCulture);

            var result = factory.Qualifies(typeof(DateTime));

            result.Should().BeTrue();
        }

        [Fact]
        public void Qualifies_WhenTypeIsNullableDateTime_ReturnsTrue()
        {
            var factory = new DateTimeParameterValueFactory(CultureInfo.InvariantCulture);

            var result = factory.Qualifies(typeof(DateTime?));

            result.Should().BeTrue();
        }

        [Fact]
        public void Qualifies_WhenTypeIsNotDateTime_ReturnsFalse()
        {
            var factory = new DateTimeParameterValueFactory(CultureInfo.InvariantCulture);

            var result = factory.Qualifies(typeof(string));

            result.Should().BeFalse();
        }

        [Fact]
        public void Create_WhenParameterIs_MMddyyyy_String_ReturnsDateTime()
        {
            var factory = new DateTimeParameterValueFactory(CultureInfo.InvariantCulture);

            var result = factory.Create(typeof(DateTime), new ExecutableParameterInfo("value", "01/01/2018"));

            result.Should().Be(new DateTime(2018, 1, 1));
        }

        [Fact]
        public void Create_WhenParameterIs_Mdyyyy_String_ReturnsDateTime()
        {
            var factory = new DateTimeParameterValueFactory(CultureInfo.InvariantCulture);

            var result = factory.Create(typeof(DateTime), new ExecutableParameterInfo("value", "1/1/2018"));

            result.Should().Be(new DateTime(2018, 1, 1));
        }

        [Fact]
        public void Create_WhenParameterIs_MMddyyyyhhmmss_String_ReturnsDateTime()
        {
            var factory = new DateTimeParameterValueFactory(CultureInfo.InvariantCulture);

            var result = factory.Create(typeof(DateTime), new ExecutableParameterInfo("value", "01/01/2018 01:01:01"));

            result.Should().Be(new DateTime(2018, 1, 1, 1, 1, 1));
        }
        
        [Fact]
        public void Create_WhenParameterIsNull_ReturnsNull()
        {
            var factory = new DateTimeParameterValueFactory(new CultureInfo("en-US"));

            var result = factory.Create(typeof(DateTime?), new ExecutableParameterInfo("value", null));

            result.Should().BeNull();
        }
    }
}