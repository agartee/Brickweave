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
using Xunit;

namespace Brickweave.Messaging.ServiceBus.Tests
{
    [Trait("Category", "Integration")]
    [Collection("ServiceBusTestCollection")]
    public class ServiceBusDomainMessengerTests : IClassFixture<ServiceBusFixture>
    {
        private readonly ServiceBusFixture _fixture;
        private readonly IMessageSerializer _serializer;
        private readonly IMessageEncoder _encoder;

        public ServiceBusDomainMessengerTests(ServiceBusFixture fixture)
        {
            _fixture = fixture;
            _serializer = new JsonMessageSerializer();
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
                    new MessageSenderRegistration(_fixture.Queue, _fixture.CreateQueueSender())
                },
                new IMessageTypeRegistration[] { },
                Enumerable.Empty<IMessageFailureHandler>(),
                new DefaultTopicOrQueueRegistration(_fixture.Queue));

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
                    new MessageSenderRegistration(_fixture.Queue, _fixture.CreateQueueSender())
                },
                new IMessageTypeRegistration[] {
                    new MessageTypeRegistration<TestDomainEvent>(_fixture.Queue)
                },
                Enumerable.Empty<IMessageFailureHandler>(),
                new DefaultTopicOrQueueRegistration("notused"));

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
