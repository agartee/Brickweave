using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Brickweave.Messaging.Serialization;
using Brickweave.Messaging.ServiceBus.Models;
using Brickweave.Messaging.ServiceBus.Tests.Fixtures;
using Brickweave.Messaging.ServiceBus.Tests.Models;
using FluentAssertions;
using Microsoft.Azure.ServiceBus;
using Xunit;

namespace Brickweave.Messaging.ServiceBus.Tests
{
    [Trait("Category", "Integration")]
    [Collection("ServiceBusTestCollection")]
    public class ServiceBusDomainMessengerTests : IClassFixture<ServiceBusFixture>
    {
        private readonly ServiceBusFixture _fixture;

        public ServiceBusDomainMessengerTests(ServiceBusFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Send_WhenMessageTypeNotRegistered_SendsMessageToServiceBusOnDefaultTopic()
        {
            var id = Guid.NewGuid();
            var serializer = new JsonMessageSerializer();
            var encoder = new Utf8Encoder();
            var messenger = new ServiceBusDomainMessenger(serializer, encoder,
                new [] { new GlobalUserPropertyStrategy("Id") }, 
                new [] { new MessageSenderRegistration(_fixture.Topic, _fixture.CreateSender()) },
                new IMessageTypeRegistration[] { },
                Enumerable.Empty<IMessageFailureHandler>(),
                new DefaultTopicOrQueueRegistration(_fixture.Topic));
    
            var client = _fixture.CreateClient(id);

            TestDomainEvent domainEvent = null;
            client.RegisterMessageHandler((msg, token) =>
            {
                var json = Encoding.UTF8.GetString(msg.Body);
                domainEvent = serializer.DeserializeObject<TestDomainEvent>(json);
                
                return Task.CompletedTask;
            }, new MessageHandlerOptions(args => Task.CompletedTask)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            });

            await messenger.SendAsync(new TestDomainEvent(id));

            while (domainEvent?.Id != id)
            {
                Thread.Sleep(2000);
            }

            domainEvent.Id.Should().Be(id);
        }

        [Fact]
        public async Task Send_WhenMessageTypeRegistered_SendsMessageToServiceBusOnRegisteredTopic()
        {
            var id = Guid.NewGuid();
            var serializer = new JsonMessageSerializer();
            var encoder = new Utf8Encoder();
            var messenger = new ServiceBusDomainMessenger(serializer, encoder,
                new[] { new GlobalUserPropertyStrategy("Id") },
                new[] { new MessageSenderRegistration(_fixture.Topic, _fixture.CreateSender()) },
                new IMessageTypeRegistration[] { new MessageTypeRegistration<TestDomainEvent>(_fixture.Topic) },
                Enumerable.Empty<IMessageFailureHandler>(),
                new DefaultTopicOrQueueRegistration("notused"));

            var client = _fixture.CreateClient(id);

            TestDomainEvent domainEvent = null;
            client.RegisterMessageHandler((msg, token) =>
            {
                var json = Encoding.UTF8.GetString(msg.Body);
                domainEvent = serializer.DeserializeObject<TestDomainEvent>(json);

                return Task.CompletedTask;
            }, new MessageHandlerOptions(args => Task.CompletedTask)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            });

            await messenger.SendAsync(new TestDomainEvent(id));

            while (domainEvent?.Id != id)
            {
                Thread.Sleep(2000);
            }

            domainEvent.Id.Should().Be(id);
        }
    }
}
