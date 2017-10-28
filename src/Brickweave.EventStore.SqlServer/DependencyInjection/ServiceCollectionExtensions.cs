using System.Linq;
using System.Reflection;
using Brickweave.EventStore.Factories;
using Brickweave.EventStore.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.EventStore.SqlServer.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventStore(this IServiceCollection services, string connectionString, 
            params Assembly[] domainAssemblies)
        {
            var shortHandTypes = domainAssemblies.SelectMany(a => a.ExportedTypes)
                .Where(t => typeof(IAggregateEvent).IsAssignableFrom(t))
                .ToArray();
            
            services
                .AddScoped<IDocumentSerializer>(s => new JsonDocumentSerializer(shortHandTypes))
                .AddScoped<IAggregateFactory, ReflectionConventionAggregateFactory>()
                .AddDbContext<EventStoreContext>(options => options.UseSqlServer(connectionString));

            return services;
        }
    }
}
