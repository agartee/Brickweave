using System;
using System.Collections.Generic;
using Brickweave.Domain.Serialization;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NSubstitute;
using Xunit;

namespace Brickweave.Domain.Tests.Serialization
{
    public class IdConverterTests
    {
        [Fact]
        public void CanConvert_WithValidType_ReturnsTrue()
        {
            var converter = new ValueObjectConverter();

            converter.CanConvert(typeof(ValueObject<int>)).Should().BeTrue();
            converter.CanConvert(typeof(ValueObject<string>)).Should().BeTrue();
            converter.CanConvert(typeof(ValueObject<Guid>)).Should().BeTrue();
        }

        [Fact]
        public void CanConvert_WithInvalidType_ReturnsFalse()
        {
            var converter = new ValueObjectConverter();
            converter.CanConvert(typeof(int)).Should().BeFalse();
        }

        [Fact]
        public void WriteJson_WithIdObject_WritesCorrectJson()
        {
            var obj = new MyObject { Id = new MyId(new Guid("b2f282a9-d6d7-4929-bb18-9fcc57c2cbd9")) };

            var json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter> { new ValueObjectConverter() }
            });

            json.Should().Be("{\"id\":\"b2f282a9-d6d7-4929-bb18-9fcc57c2cbd9\"}");
        }

        [Fact]
        public void ReadJson_Throws()
        {
            var converter = new ValueObjectConverter();

            Assert.Throws<NotSupportedException>(() => converter.ReadJson(
                Arg.Any<JsonReader>(),
                Arg.Any<Type>(),
                Arg.Any<object>(),
                Arg.Any<JsonSerializer>()));
        }

        public class MyId : ValueObject<Guid>
        {
            public MyId(Guid value) : base(value)
            {
            }
        }

        public class MyObject
        {
            public MyId Id { get; set; }
        }
    }
}
