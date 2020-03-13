using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.EventStore.SqlServer.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static EventStoreOptionsBuilder AddEventStore(this IServiceCollection services)
        {
            return new EventStoreOptionsBuilder(services);
        }
    }
}
