namespace Brickweave.Messaging.ServiceBus.Models
{
    public class DefaultTopicOrQueueRegistration
    {
        public DefaultTopicOrQueueRegistration(string topicOrQueue)
        {
            TopicOrQueue = topicOrQueue;
        }

        public string TopicOrQueue { get; }
    }
}
