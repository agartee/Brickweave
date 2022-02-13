using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Brickweave.Messaging.ServiceBus.Models;
using Brickweave.Messaging.ServiceBus.Tests.Fixtures;
using Brickweave.Messaging.ServiceBus.Tests.Models;
using Brickweave.Messaging.Services;
using Brickweave.Serialization;
using FluentAssertions;
using Xunit;

namespace Brickweave.Messaging.ServiceBus.Services.Tests
{
    [Trait("Category", "Integration")]
    [Collection("ServiceBusTestCollection")]
    public class ServiceBusDomainMessengerTests : IClassFixture<ServiceBusFixture>
    {
        private readonly ServiceBusFixture _fixture;
        private readonly IDocumentSerializer _serializer;
        private readonly IMessageEncoder _encoder;

        public ServiceBusDomainMessengerTests(ServiceBusFixture fixture)
        {
            _fixture = fixture;
            _serializer = new JsonDocumentSerializer();
            _encoder = new Utf8Encoder();
        }

        [Fact]
        public async Task Send_WhenMessageTypeNotRegistered_SendsMessageToServiceBusOnDefaultTopicOrQueue()
        {
            var id = Guid.NewGuid();

            var messenger = new ServiceBusDomainMessenger(_serializer, _encoder,
                new[] {
                    new GlobalUserPropertyStrategy("Id")
                },
                new[] {
                    new MessageSenderRegistration("test", _fixture.CreateQueueSender())
                },
                new IMessageTypeRegistration[] { },
                Enumerable.Empty<IMessageFailureHandler>(),
                new DefaultMessageSenderRegistration("test"));

            var client = _fixture.CreateQueueClient();

            TestDomainEvent domainEvent = null;
            client.RegisterMessageHandler((msg, token) =>
            {
                var json = Encoding.UTF8.GetString(msg.Body);
                domainEvent = _serializer.DeserializeObject<TestDomainEvent>(json);

                return Task.CompletedTask;
            }, _fixture.CreateMessageHandlerOptions());

            await messenger.SendAsync(new TestDomainEvent(id));

            while (domainEvent?.Id != id)
            {
                Thread.Sleep(1000);
            }

            await client.CloseAsync();

            domainEvent.Id.Should().Be(id);
        }

        [Fact]
        public async Task Send_WhenMessageTypeRegistered_SendsMessageToServiceBusOnRegisteredTopicOrQueue()
        {
            var id = Guid.NewGuid();

            var messenger = new ServiceBusDomainMessenger(_serializer, _encoder,
                new[] {
                    new GlobalUserPropertyStrategy("Id")
                },
                new[] {
                    new MessageSenderRegistration("test", _fixture.CreateQueueSender())
                },
                new IMessageTypeRegistration[] {
                    new MessageTypeRegistration<TestDomainEvent>("test")
                },
                Enumerable.Empty<IMessageFailureHandler>(),
                new DefaultMessageSenderRegistration("test-ignored"));

            var client = _fixture.CreateQueueClient();

            TestDomainEvent domainEvent = null;
            client.RegisterMessageHandler((msg, token) =>
            {
                var json = Encoding.UTF8.GetString(msg.Body);
                domainEvent = _serializer.DeserializeObject<TestDomainEvent>(json);

                return Task.CompletedTask;
            }, _fixture.CreateMessageHandlerOptions());

            await messenger.SendAsync(new TestDomainEvent(id));

            while (domainEvent?.Id != id)
            {
                Thread.Sleep(1000);
            }

            await client.CloseAsync();

            domainEvent.Id.Should().Be(id);
        }
    }
}
