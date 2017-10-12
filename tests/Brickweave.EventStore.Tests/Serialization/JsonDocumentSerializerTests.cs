using System;
using Brickweave.EventStore.Serialization;
using Brickweave.EventStore.Tests.Models;
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
            var obj = new TestAggregateCreated(idValue);
            var serializer = new JsonDocumentSerializer(typeof(TestAggregateCreated));

            var json = serializer.SerializeObject(obj);

            json.Should().Contain("\"$type\": \"TestAggregateCreated\"");

            _output.WriteLine(json);
        }

        [Fact]
        public void SerializeObject_WhenObjectIsNotRegisteredAsShorthandType_ReturnsJson()
        {
            var idValue = Guid.NewGuid();
            var obj = new TestAggregateCreated(idValue);
            var serializer = new JsonDocumentSerializer();

            var json = serializer.SerializeObject(obj);

            json.Should().Contain("\"$type\": \"Brickweave.EventStore.Tests.Models.TestAggregateCreated, Brickweave.EventStore.Tests\"");

            _output.WriteLine(json);
        }
    }
}
