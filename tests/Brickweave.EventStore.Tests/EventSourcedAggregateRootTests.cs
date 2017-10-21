using System.Linq;
using Brickweave.EventStore.Exceptions;
using Brickweave.EventStore.Tests.Models;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Brickweave.EventStore.Tests
{
    public class EventSourcedAggregateRootTests
    {
        private readonly ITestOutputHelper _output;

        public EventSourcedAggregateRootTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void ApplyEvent_WhenEventIsRegisteredWithAggregate_UpdatesAggregateState()
        {
            var id = TestId.NewId();
            var aggregate = new TestAggregate(id);

            aggregate.TestId.Should().Be(id);
        }

        [Fact]
        public void ApplyEvent_WhenAggregateHasNoEventRegistrations_Throws()
        {
            var exception = Assert.Throws<EventHandlersNoFoundException>(
                () => new TestAggregateWithoutEventRegistrations(TestId.NewId()));

            exception.AggregateType.Should().Be(typeof(TestAggregateWithoutEventRegistrations));

            _output.WriteLine(exception.Message);
        }

        [Fact]
        public void ApplyEvent_WhenEventIsNotRegisteredWithAggregate_Throws()
        {
            var aggregate = new TestAggregate(TestId.NewId());

            var exception = Assert.Throws<EventHandlerNotFoundException>(
                () => aggregate.RaiseUnregisteredEvent());

            exception.AggregateType.Should().Be(typeof(TestAggregate));

            _output.WriteLine(exception.Message);
        }
        
        [Fact]
        public void GetUncommittedEvents_ReturnsCopyOfAggregateUncommittedEvents()
        {
            var id = TestId.NewId();
            var aggregate = new TestAggregate(id);

            var uncommittedEvents = aggregate.GetUncommittedEvents().ToArray();

            uncommittedEvents.Should().HaveCount(1);
            uncommittedEvents.First().Should().BeOfType<TestAggregateCreated>();
            uncommittedEvents.Cast<TestAggregateCreated>().First().TestId.Should().Be(id.Value);
        }

        [Fact]
        public void ClearUncommittedEvents_RemovesAllUncommitedEvents()
        {
            var aggregate = new TestAggregate(TestId.NewId());

            aggregate.ClearUncommittedEvents();

            aggregate.GetUncommittedEvents().Should().BeEmpty();
        }
    }
}
