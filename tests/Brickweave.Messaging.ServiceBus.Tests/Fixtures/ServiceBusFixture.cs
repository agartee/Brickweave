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
        }

        private string ConnectionString => _config.GetConnectionString("serviceBus");
        private string Subscription => _config["serviceBusSubscription"];
        public string Topic => _config["serviceBusTopic"];

        public IMessageSender CreateSender()
        {
            return new MessageSender(ConnectionString, Topic, RetryPolicy.Default);
        }

        public ISubscriptionClient CreateClient(Guid id)
        {
            return new SubscriptionClient(ConnectionString, Topic, Subscription,
                ReceiveMode.ReceiveAndDelete);
        }
    }
}
