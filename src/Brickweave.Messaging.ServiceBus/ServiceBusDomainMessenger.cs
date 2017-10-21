using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brickweave.Messaging.Serialization;
using Brickweave.Messaging.ServiceBus.Extensions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;

namespace Brickweave.Messaging.ServiceBus
{
    public class ServiceBusDomainMessenger : IDomainMessenger
    {
        private readonly IEnumerable<IUserPropertyStrategy> _userPropertyStrategies;
        private readonly IMessageSerializer _serializer;
        private readonly IMessageEncoder _encoder;
        private readonly IMessageSender _sender;
    
        public ServiceBusDomainMessenger(IEnumerable<IUserPropertyStrategy> userPropertyStrategies, 
            IMessageSerializer serializer, IMessageEncoder encoder, IMessageSender sender)
        {
            _userPropertyStrategies = userPropertyStrategies;
            _serializer = serializer;
            _sender = sender;
            _encoder = encoder;
        }

        public async Task SendAsync(IDomainEvent @event)
        {
            await SendAsync(new List<IDomainEvent> { @event });
        }

        public async Task SendAsync(params IDomainEvent[] events)
        {
            await SendAsync(events.ToList());
        }

        public async Task SendAsync(IEnumerable<IDomainEvent> events)
        {
            foreach (var domainEvent in events)
                await _sender.SendAsync(BuildBrokeredMessage(domainEvent));
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
