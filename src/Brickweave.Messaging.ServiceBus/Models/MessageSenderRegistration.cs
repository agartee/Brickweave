using Microsoft.Azure.ServiceBus.Core;

namespace Brickweave.Messaging.ServiceBus.Models
{
    public class MessageSenderRegistration
    {
        public MessageSenderRegistration(string name, IMessageSender messageSender)
        {
            Name = name;
            MessageSender = messageSender;
        }

        public string Name { get; }
        public IMessageSender MessageSender { get; }
    }
}
