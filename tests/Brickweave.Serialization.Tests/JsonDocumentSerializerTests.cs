using System;
using Brickweave.Serialization;
using Brickweave.Serialization.Tests.Models;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Brickweave.EventStore.Tests.Serialization
{
    public class JsonDocumentSerializerTests
    {
        private readonly ITestOutputHelper _output;

        public JsonDocumentSerializerTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void SerializeObject_WhenObjectIsRegisteredAsShorthandType_ReturnsJson()
        {
            var idValue = Guid.NewGuid();
            var obj = new TestClass(idValue);
            var serializer = new JsonDocumentSerializer(typeof(TestClass));

            var json = serializer.SerializeObject(obj);

            json.Should().Contain("\"$type\":\"TestClass\"");

            _output.WriteLine(json);
        }

        [Fact]
        public void SerializeObject_WhenObjectIsNotRegisteredAsShorthandType_ReturnsJson()
        {
            var idValue = Guid.NewGuid();
            var obj = new TestClass(idValue);
            var serializer = new JsonDocumentSerializer();

            var json = serializer.SerializeObject(obj);

            json.Should().Contain("\"$type\":\"Brickweave.Serialization.Tests.Models.TestClass, Brickweave.Serialization.Tests\"");

            _output.WriteLine(json);
        }
    }
}
