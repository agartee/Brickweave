using System;
using System.Collections.Generic;
using System.Linq;
using Brickweave.EventStore.Exceptions;
using Brickweave.EventStore.Tests.Models;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Brickweave.EventStore.Tests
{
    public class EventSourcedEntityTests
    {
        private readonly ITestOutputHelper _output;

        public EventSourcedEntityTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void ApplyEvent_WhenEventIsRegisteredWithAggregate_UpdatesAggregateState()
        {
            var id = Guid.NewGuid();
            var aggregate = new TestAggregate(id);

            aggregate.Id.Should().Be(id);
        }

        [Fact]
        public void ApplyEvent_WithMultipleEvents_EnqueuesEventsChronologically()
        {
            var id = Guid.NewGuid();
            var aggregate = new TestAggregate(id);
            aggregate.RaiseMiscEvent();

            var events = aggregate.GetUncommittedEvents();
            events.Should().HaveCount(2);
            events.First().Should().BeOfType<TestAggregateCreated>();
            events.Last().Should().BeOfType<MiscEvent>();
        }

        [Fact]
        public void ApplyEvent_WhenAggregateHasNoEventRegistrations_Throws()
        {
            var exception = Assert.Throws<EventHandlersNoFoundException>(
                () => new TestAggregateWithoutEventRegistrations(Guid.NewGuid()));

            exception.AggregateType.Should().Be(typeof(TestAggregateWithoutEventRegistrations));

            _output.WriteLine(exception.Message);
        }

        [Fact]
        public void ApplyEvent_WhenEventIsNotRegisteredWithAggregate_Throws()
        {
            var aggregate = new TestAggregate(Guid.NewGuid());

            var exception = Assert.Throws<EventHandlerNotFoundException>(
                () => aggregate.RaiseUnregisteredEvent());

            exception.AggregateType.Should().Be(typeof(TestAggregate));

            _output.WriteLine(exception.Message);
        }
        
        [Fact]
        public void AddAndModifyChild_UpdatesListAndAddsEvents()
        {
            var aggregate = new TestAggregate(Guid.NewGuid());
            aggregate.ClearUncommittedEvents();
            
            aggregate.AddChild(1);

            var comment = "Adam was here";
            var child = aggregate.Children.First();
            child.SetComment(comment);
            child.Comment.Should().Be(comment);

            var uncommittedEvents = aggregate.GetUncommittedEvents().ToArray();

            uncommittedEvents.Should().HaveCount(2);
            uncommittedEvents.First().Should().BeOfType<ChildAdded>();
            uncommittedEvents.Last().Should().BeOfType<ChildCommentSet>();
        }
        
        [Fact]
        public void ApplyEvents_WithChildEvents_RoutesEventsCorrectly()
        {
            var aggregateId = Guid.NewGuid();
            var childId = 1;
            var comment = "hi.";

            var events = new List<IEvent>
            {
                new TestAggregateCreated(aggregateId),
                new ChildAdded(childId),
                new ChildCommentSet(childId, comment)
            };

            var aggregate = new TestAggregate(events);

            aggregate.Id.Should().Be(aggregateId);
            aggregate.Children.Should().HaveCount(1);

            var child = aggregate.Children.First();
            child.Id.Should().Be(childId);
            child.Comment.Should().Be(comment);
        }

        [Fact]
        public void GetUncommittedEvents_ReturnsCopyOfAggregateUncommittedEvents()
        {
            var id = Guid.NewGuid();
            var aggregate = new TestAggregate(id);

            var uncommittedEvents = aggregate.GetUncommittedEvents().ToArray();

            uncommittedEvents.Should().HaveCount(1);
            uncommittedEvents.First().Should().BeOfType<TestAggregateCreated>();
            uncommittedEvents.Cast<TestAggregateCreated>().First().TestId.Should().Be(id);
        }

        [Fact]
        public void ClearUncommittedEvents_RemovesAllUncommitedEvents()
        {
            var aggregate = new TestAggregate(Guid.NewGuid());

            aggregate.ClearUncommittedEvents();

            aggregate.GetUncommittedEvents().Should().BeEmpty();
        }
    }
}
