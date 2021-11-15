using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brickweave.Domain;
using Brickweave.Messaging.ServiceBus.Exceptions;
using Brickweave.Messaging.ServiceBus.Extensions;
using Brickweave.Messaging.ServiceBus.Models;
using Brickweave.Messaging.Services;
using Brickweave.Serialization;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;

namespace Brickweave.Messaging.ServiceBus.Services
{
    public class ServiceBusDomainMessenger : IDomainMessenger
    {
        private readonly IDocumentSerializer _serializer;
        private readonly IMessageEncoder _encoder;

        private readonly IEnumerable<IUserPropertyStrategy> _userPropertyStrategies;
        private readonly IEnumerable<MessageSenderRegistration> _messageSenderRegistrations;
        private readonly IEnumerable<IMessageTypeRegistration> _messageTypeRegistrations;
        private readonly IEnumerable<IMessageFailureHandler> _messageFailureHandlers;

        private readonly DefaultTopicOrQueueRegistration _defaultTopicOrQueueRegistration;

        public ServiceBusDomainMessenger(IDocumentSerializer serializer, IMessageEncoder encoder,
            IEnumerable<IUserPropertyStrategy> userPropertyStrategies,
            IEnumerable<MessageSenderRegistration> messageSenderRegistrations,
            IEnumerable<IMessageTypeRegistration> messageTypeRegistrations,
            IEnumerable<IMessageFailureHandler> messageFailureHandlers,
            DefaultTopicOrQueueRegistration defaultTopicOrQueueRegistration)
        {
            _serializer = serializer;
            _encoder = encoder;
            _userPropertyStrategies = userPropertyStrategies;
            _messageSenderRegistrations = messageSenderRegistrations;
            _messageTypeRegistrations = messageTypeRegistrations;
            _messageFailureHandlers = messageFailureHandlers;
            _defaultTopicOrQueueRegistration = defaultTopicOrQueueRegistration;
        }

        public async Task SendAsync(IDomainEvent @event)
        {
            await SendAsync(new List<IDomainEvent> { @event });
        }

        public async Task SendAsync(IEnumerable<IDomainEvent> events)
        {
            await SendAsync(events.ToArray());
        }

        public async Task SendAsync(params IDomainEvent[] events)
        {
            var exceptions = new List<Exception>();

            foreach (var domainEvent in events)
            {
                try
                {
                    await GetSender(domainEvent.GetType())
                        .SendAsync(BuildBrokeredMessage(domainEvent));
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);

                    _messageFailureHandlers.ToList()
                        .ForEach(async h => await h.Handle(domainEvent, ex));
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException("One or more errors occurred while sending domain messages.",
                    exceptions);
            }
        }

        private IMessageSender GetSender(Type messageType)
        {
            var messageTopicOrQueue = _messageTypeRegistrations
                .FirstOrDefault(r => r.MessageType == messageType)?.TopicOrQueue;

            if (messageTopicOrQueue == null)
                return GetDefaultSender();

            var sender = _messageSenderRegistrations
                .FirstOrDefault(r => r.TopicOrQueue == messageTopicOrQueue)?.MessageSender;

            if (sender == null)
                throw new MessageSenderNotRegisteredException(messageTopicOrQueue);

            return sender;
        }

        private IMessageSender GetDefaultSender()
        {
            var sender = _messageSenderRegistrations
                .FirstOrDefault(r => r.TopicOrQueue == _defaultTopicOrQueueRegistration.TopicOrQueue)?.MessageSender;

            if(sender == null)
                throw new DefaultTopicOrQueueNotRegisteredException();

            return sender;
        }

        private Message BuildBrokeredMessage(IDomainEvent domainEvent)
        {
            var json = _serializer.SerializeObject(domainEvent);
            var message = new Message(_encoder.Encode(json))
            {
                ContentType = domainEvent.GetType().Name
            };

            var userProperties = _userPropertyStrategies
                .SelectMany(s => s.GetUserProperties(domainEvent))
                .ToList();

            userProperties.ForEach(kvp => message.UserProperties.TryAdd(kvp));
            message.UserProperties.Add("Encoding", _encoder.Name);

            return message;
        }
    }
}
