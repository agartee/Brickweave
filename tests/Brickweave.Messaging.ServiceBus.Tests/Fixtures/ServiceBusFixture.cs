using System;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;

namespace Brickweave.Messaging.ServiceBus.Tests.Fixtures
{
    public class ServiceBusFixture
    {
        private readonly IConfiguration _config;

        public ServiceBusFixture()
        {
            _config = new ConfigurationBuilder()
                .AddUserSecrets<ServiceBusFixture>()
                .AddEnvironmentVariables()
                .Build();
        }

        private string ConnectionString => _config.GetConnectionString("serviceBus");
        private string Subscription => _config["messaging:subscription_tests"];
        public string Topic => _config["messaging:topic_tests"];
        public string Queue => _config["messaging:queue_tests"];

        public IMessageSender CreateTopicSender()
        {
            return new MessageSender(ConnectionString, Topic, RetryPolicy.Default);
        }

        public IMessageSender CreateQueueSender()
        {
            return new MessageSender(ConnectionString, Queue, RetryPolicy.Default);
        }

        public ISubscriptionClient CreateSubscriptionClient()
        {
            return new SubscriptionClient(ConnectionString, Topic, Subscription,
                ReceiveMode.ReceiveAndDelete);
        }

        public IQueueClient CreateQueueClient()
        {
            return new QueueClient(ConnectionString, Queue,
                ReceiveMode.ReceiveAndDelete);
        }

        public IMessageReceiver CreateQueueMessageReceiver()
        {
            return new MessageReceiver(ConnectionString, Queue, 
                ReceiveMode.PeekLock);
        }

        public MessageHandlerOptions CreateMessageHandlerOptions()
        {
            return new MessageHandlerOptions(args => Task.CompletedTask)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
        }
    }
}
