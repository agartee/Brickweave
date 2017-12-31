using System;
using System.Globalization;
using Brickweave.Cqrs.Cli.Factories;
using FluentAssertions;
using Xunit;

namespace Brickweave.Cqrs.Cli.Tests.Factories
{
    public class DateTimeParameterFactoryTests
    {
        [Fact]
        public void Qualifies_WhenTypeIsDateTime_ReturnsTrue()
        {
            var factory = new DateTimeParameterFactory(CultureInfo.InvariantCulture);

            var result = factory.Qualifies(typeof(DateTime));

            result.Should().BeTrue();
        }

        [Fact]
        public void Qualifies_WhenTypeIsNullableDateTime_ReturnsTrue()
        {
            var factory = new DateTimeParameterFactory(CultureInfo.InvariantCulture);

            var result = factory.Qualifies(typeof(DateTime?));

            result.Should().BeTrue();
        }

        [Fact]
        public void Qualifies_WhenTypeIsNotDateTime_ReturnsFalse()
        {
            var factory = new DateTimeParameterFactory(CultureInfo.InvariantCulture);

            var result = factory.Qualifies(typeof(string));

            result.Should().BeFalse();
        }

        [Fact]
        public void Create_WhenParameterIs_MMddyyyy_String_ReturnsDateTime()
        {
            var factory = new DateTimeParameterFactory(CultureInfo.InvariantCulture);

            var result = factory.Create(typeof(DateTime), "01/01/2018");

            result.Should().Be(new DateTime(2018, 1, 1));
        }

        [Fact]
        public void Create_WhenParameterIs_Mdyyyy_String_ReturnsDateTime()
        {
            var factory = new DateTimeParameterFactory(CultureInfo.InvariantCulture);

            var result = factory.Create(typeof(DateTime), "1/1/2018");

            result.Should().Be(new DateTime(2018, 1, 1));
        }

        [Fact]
        public void Create_WhenParameterIs_MMddyyyyhhmmss_String_ReturnsDateTime()
        {
            var factory = new DateTimeParameterFactory(CultureInfo.InvariantCulture);

            var result = factory.Create(typeof(DateTime), "01/01/2018 01:01:01");

            result.Should().Be(new DateTime(2018, 1, 1, 1, 1, 1));
        }
        
        [Fact]
        public void Create_WhenParameterIsNull_ReturnsNull()
        {
            var factory = new DateTimeParameterFactory(new CultureInfo("en-US"));

            var result = factory.Create(typeof(DateTime?), null);

            result.Should().BeNull();
        }
    }
}