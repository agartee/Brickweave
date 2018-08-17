using System;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;

namespace Brickweave.Messaging.ServiceBus.Tests.Fixtures
{
    public class ServiceBusFixture
    {
        private readonly IConfiguration _config;

        private readonly Func<Guid, ISubscriptionClient> _createClient;

        public ServiceBusFixture()
        {
            _config = new ConfigurationBuilder()
                .AddUserSecrets<ServiceBusFixture>()
                .AddEnvironmentVariables()
                .Build();
                        
            Sender = new MessageSender(ConnectionString, Topic, RetryPolicy.Default);
        }

        private string ConnectionString => _config.GetConnectionString("serviceBus");
        private string Topic => _config["serviceBusTopic"];
        private string Subscription => _config["serviceBusSubscription"];

        public IMessageSender Sender { get; }

        public ISubscriptionClient CreateClient(Guid id)
        {
            return new SubscriptionClient(ConnectionString, Topic, Subscription,
                ReceiveMode.ReceiveAndDelete);
        }

        
    }
}
