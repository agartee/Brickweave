using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.Messaging.ServiceBus.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ServiceBusOptionsBuilder AddMessageBus(this IServiceCollection services)
        {
            return new ServiceBusOptionsBuilder(services);
        }
    }
}
