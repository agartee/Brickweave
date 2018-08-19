using System;

namespace Brickweave.Messaging.ServiceBus.Exceptions
{
    public class MessageSenderNotRegisteredException : Exception
    {
        private const string MESSAGE = "Message sender not registered for topic or queue, \"{0}\"";

        public MessageSenderNotRegisteredException(string topicOrQueue) : base(
            String.Format(MESSAGE, topicOrQueue))
        {
            TopicOrQueue = topicOrQueue;
        }

        public string TopicOrQueue { get; }
    }
}
