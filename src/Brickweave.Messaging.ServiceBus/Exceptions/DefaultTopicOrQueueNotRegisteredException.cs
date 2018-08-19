using System;

namespace Brickweave.Messaging.ServiceBus.Exceptions
{
    public class DefaultTopicOrQueueNotRegisteredException : Exception
    {
        private const string MESSAGE = "No default messager topic or queue is registered";

        public DefaultTopicOrQueueNotRegisteredException() : base(
            String.Format(MESSAGE))
        {
        }
    }
}
