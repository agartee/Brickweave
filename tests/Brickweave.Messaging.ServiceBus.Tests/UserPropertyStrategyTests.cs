using System;
using System.Collections.Generic;
using Brickweave.Messaging.ServiceBus.Tests.Models;
using FluentAssertions;
using Xunit;

namespace Brickweave.Messaging.ServiceBus.Tests
{
    public class UserPropertyStrategyTests
    {
        [Fact]
        public void GetUserProperties_WhenDomainEventIsOfRegisteredType_ReturnsProperties()
        {
            var strategy = new UserPropertyStrategy<TestDomainEvent>(e => 
                new Dictionary<string, object>
                {
                   ["Id"] = e.Id
                });

            var id = Guid.NewGuid();
            var results = strategy.GetUserProperties(new TestDomainEvent(id));

            results.Should().HaveCount(1);
            results["Id"].Should().Be(id);
        }

        [Fact]
        public void GetUserProperties_WhenDomainEventIsNotOfRegisteredType_ReturnsEmptyDictionary()
        {
            var strategy = new UserPropertyStrategy<TestDomainEvent>(e =>
                new Dictionary<string, object>
                {
                    ["Id"] = e.Id
                });

            var id = Guid.NewGuid();
            var results = strategy.GetUserProperties(new AnotherTestDomainEvent(id));

            results.Should().BeEmpty();
        }
    }
}
