using System;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using ISubscriptionClient = Microsoft.Azure.ServiceBus.ISubscriptionClient;
using SubscriptionClient = Microsoft.Azure.ServiceBus.SubscriptionClient;

namespace Brickweave.Messaging.ServiceBus.Tests.Fixtures
{
    public class ServiceBusFixture
    {
        private readonly Func<Guid, ISubscriptionClient> _createClient;

        public ServiceBusFixture()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddUserSecrets<ServiceBusFixture>()
                .AddEnvironmentVariables()
                .Build();

            var connectionString = config.GetConnectionString("serviceBus");

            Sender = new MessageSender(
                connectionString,
                config["serviceBusTopic"], 
                RetryPolicy.Default);

            _createClient = (id) =>
            {
                var client = new SubscriptionClient(
                    connectionString,
                    config["serviceBusTopic"],
                    config["serviceBusSubscription"],
                    ReceiveMode.ReceiveAndDelete);

                return client;
            };
        }
        
        public IMessageSender Sender { get; }

        public ISubscriptionClient CreateClient(Guid id) => 
            _createClient.Invoke(id);
    }
}
