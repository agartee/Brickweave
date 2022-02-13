using System;

namespace Brickweave.Messaging.ServiceBus.Models
{
    public interface IMessageTypeRegistration
    {
        Type MessageType { get; }
        string MessageSenderName { get; }
    }
}
