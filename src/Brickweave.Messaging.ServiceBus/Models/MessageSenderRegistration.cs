using Microsoft.Azure.ServiceBus.Core;

namespace Brickweave.Messaging.ServiceBus.Models
{
    public class MessageSenderRegistration
    {
        public MessageSenderRegistration(string topicOrQueue, IMessageSender messageSender)
        {
            TopicOrQueue = topicOrQueue;
            MessageSender = messageSender;
        }

        public string TopicOrQueue { get; }
        public IMessageSender MessageSender { get; }
    }
}
