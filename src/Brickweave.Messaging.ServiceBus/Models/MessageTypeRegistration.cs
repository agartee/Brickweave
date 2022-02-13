using System;
using Brickweave.Domain;

namespace Brickweave.Messaging.ServiceBus.Models
{
    public class MessageTypeRegistration<TType> : IMessageTypeRegistration where TType : IDomainEvent
    {
        public MessageTypeRegistration(string messageSenderName)
        {
            MessageType = typeof(TType);
            MessageSenderName = messageSenderName;
        }

        public Type MessageType { get; }
        public string MessageSenderName { get; }
    }
}
