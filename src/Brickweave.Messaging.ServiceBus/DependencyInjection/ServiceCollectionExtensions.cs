using Brickweave.Messaging.Serialization;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.Messaging.ServiceBus.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ServiceBusDependencyBuilder AddMessageBus(this IServiceCollection services,
            string connectionString, string topicOrQueue, RetryPolicy retryPolicy = null)
        {
            services
                .AddScoped<IDomainMessenger, ServiceBusDomainMessenger>()
                .AddScoped<IMessageSerializer, JsonMessageSerializer>()
                .AddScoped<IMessageSender>(s => new MessageSender(connectionString, topicOrQueue, retryPolicy ?? RetryPolicy.Default));

            return new ServiceBusDependencyBuilder(services);
        }
    }
}
