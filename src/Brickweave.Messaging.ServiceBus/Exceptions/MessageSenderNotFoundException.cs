using System;

namespace Brickweave.Messaging.ServiceBus.Exceptions
{
    public class MessageSenderNotFoundException : Exception
    {
        private const string MESSAGE = "Message sender not found and no default specified";

        public MessageSenderNotFoundException() : base(
            String.Format(MESSAGE))
        {
        }
    }
}
