using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.EventStore.SqlServer.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static EventStoreOptionsBuilder AddEventStore(this IServiceCollection services,
            params Assembly[] domainAssemblies)
        {
            return new EventStoreOptionsBuilder(services, domainAssemblies);
        }
    }
}
