using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.Messaging.ServiceBus.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ServiceBusOptionsBuilder AddBrickweaveMessaging(this IServiceCollection services)
        {
            return new ServiceBusOptionsBuilder(services);
        }
    }
}
