using System;

namespace Brickweave.Messaging.ServiceBus.Models
{
    public class MessageTypeRegistration<TType> : IMessageTypeRegistration where TType : IDomainEvent
    {
        public MessageTypeRegistration(string topicOrQueue)
        {
            MessageType = typeof(TType);
            TopicOrQueue = topicOrQueue;
        }

        public Type MessageType { get; }
        public string TopicOrQueue { get; }
    }
}
