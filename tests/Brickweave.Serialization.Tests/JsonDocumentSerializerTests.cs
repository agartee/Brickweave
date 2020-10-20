using System;
using System.Collections.Generic;
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
        public void DeserializeObject_ReturnsObject()
        {
            var shorthandTypes = new[] { typeof(TestClass) };
            var serializer = new JsonDocumentSerializer(shorthandTypes);

            var typeName = "TestClass";
            var json = "{\"id\":\"3B79C1FF-6F82-44B0-9EE2-E80920A7DD93\"}";

            var result = serializer.DeserializeObject<object>(typeName, json);

            result.Should().BeOfType<TestClass>();
            ((TestClass)result).Id.Should().Be(new Guid("3B79C1FF-6F82-44B0-9EE2-E80920A7DD93"));
        }

        [Fact]
        public void DeserializeObject_WhenObjectContainsDictionary_ReturnsObject()
        {
            var shorthandTypes = new[] { typeof(TestClassWithDictionary) };
            var serializer = new JsonDocumentSerializer(shorthandTypes);

            var id = new Guid("3B79C1FF-6F82-44B0-9EE2-E80920A7DD93");
            var obj = new TestClassWithDictionary(id, new Dictionary<string, IEnumerable<object>>
            {
                ["thing1"] = new object[] { "A", "B", "C", 1, 2, 3 }
            });

            var typeName = "TestClassWithDictionary";
            var json = serializer.SerializeObject(obj);

            var result = serializer.DeserializeObject<object>(typeName, json);

            result.Should().BeOfType<TestClassWithDictionary>();

            var testClass = result as TestClassWithDictionary;
            testClass.Id.Should().Be(new Guid("3B79C1FF-6F82-44B0-9EE2-E80920A7DD93"));
            testClass.Things["thing1"].Should().Contain(new object[] { "A", "B", "C", 1L, 2L, 3L });
        }
    }
}
