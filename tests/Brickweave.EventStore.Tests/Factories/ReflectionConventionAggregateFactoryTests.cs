using System;
using Brickweave.EventStore.Exceptions;
using Brickweave.EventStore.Factories;
using Brickweave.EventStore.Tests.Models;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Brickweave.EventStore.Tests.Factories
{
    public class ReflectionConventionAggregateFactoryTests
    {
        private readonly ITestOutputHelper _output;

        public ReflectionConventionAggregateFactoryTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Create_WhenAggregateRegisteredEventsAndConstructorExists_ReturnsAggregate()
        {
            var idValue = Guid.NewGuid();
            var events = new[] { new TestAggregateCreated(idValue) };
            var factory = new ReflectionConventionAggregateFactory();

            var aggregate = factory.Create<TestAggregate>(events);

            aggregate.TestId.Should().Be(new TestId(idValue));
        }

        [Fact]
        public void Create_WhenConstructorDoesNotExistThatTakesEvents_Throws()
        {
            var events = new[] { new TestAggregateCreated(Guid.NewGuid()) };
            var factory = new ReflectionConventionAggregateFactory();

            var exception = Assert.Throws<ConstructorNotFoundException>(
                () => factory.Create<TestAggregateWithoutEventsConstructor>(events));

            _output.WriteLine(exception.Message);
        }
        
        [Fact]
        public void Create_WhenAggregateTypeDoesNotExist_Throws()
        {
            var events = new[] { new TestAggregateCreated(Guid.NewGuid()) };
            var factory = new ReflectionConventionAggregateFactory();

            var exception = Assert.Throws<UnrecognizedTypeException>(
                () => factory.Create("BadNamespace.BadAggregate", events));

            _output.WriteLine(exception.Message);
        }
    }
}
