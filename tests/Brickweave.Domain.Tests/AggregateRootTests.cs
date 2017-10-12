using System;
using System.Linq;
using Brickweave.Domain.Tests.Models;
using FluentAssertions;
using Xunit;

namespace Brickweave.Domain.Tests
{
    public class AggregateRootTests
    {
        [Fact]
        public void EnqueueDomainEvent_WhenEventIsNotNull_StoresDomainEvent()
        {
            var model = new TestDomainModel();

            model.RaiseDomainEvent("bar");

            var enqueuedDomainEvents = model.GetEnqueuedDomainEvents().ToArray();
            enqueuedDomainEvents.Should().HaveCount(1);
            enqueuedDomainEvents.Cast<TestDomainEvent>().First().Bar.Should().Be("bar");
        }

        [Fact]
        public void EnqueueDomainEvent_WhenEventIsNull_Throws()
        {
            var model = new TestDomainModel();

            void Act() => model.NullFoo();

            Assert.Throws<ArgumentNullException>((Action) Act);
        }

        [Fact]
        public void ClearEnqueuedDomainEvents_RemovesAllEnqueuedDomainEvents()
        {
            var model = new TestDomainModel();
            model.RaiseDomainEvent("bar");

            model.ClearEnqueuedDomainEvents();

            model.GetEnqueuedDomainEvents().Should().BeEmpty();
        }
    }
}
