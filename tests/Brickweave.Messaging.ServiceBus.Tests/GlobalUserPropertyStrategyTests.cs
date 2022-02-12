using System;
using System.Linq;
using Brickweave.Messaging.ServiceBus.Tests.Models;
using FluentAssertions;
using Xunit;

namespace Brickweave.Messaging.ServiceBus.Tests
{
    public class GlobalUserPropertyStrategyTests
    {
        [Fact]
        public void GetUserProperties_WhenDomainEventContainsRegisteredPropertyName_ReturnsDictionaryWithPropertyValue()
        {
            var id = Guid.NewGuid();
            var strategy = new GlobalUserPropertyStrategy("Id");
            var domainEvent = new TestDomainEvent(id);

            var results = strategy.GetUserProperties(domainEvent).ToArray();

            results.Should().HaveCount(1);
            results.First().Key.Should().Be("Id");
            results.First().Value.Should().Be(id);
        }

        [Fact]
        public void GetUserProperties_WhenDomainEventContainsNonRegisteredPropertyName_ReturnsDictionaryWithoutPropertyValue()
        {
            var strategy = new GlobalUserPropertyStrategy();
            var domainEvent = new TestDomainEvent(Guid.NewGuid());

            var results = strategy.GetUserProperties(domainEvent).ToArray();

            results.Should().HaveCount(0);
        }
    }
}
