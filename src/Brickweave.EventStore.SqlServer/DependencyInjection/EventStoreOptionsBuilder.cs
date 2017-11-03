using System;
using System.Linq;
using System.Reflection;
using Brickweave.EventStore.Factories;
using Brickweave.EventStore.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.EventStore.SqlServer.DependencyInjection
{
    public class EventStoreOptionsBuilder
    {
        private readonly IServiceCollection _services;
        
        public EventStoreOptionsBuilder(IServiceCollection services, params Assembly[] domainAssemblies)
        {
            var shortHandTypes = domainAssemblies.SelectMany(a => a.ExportedTypes)
                .Where(t => typeof(IAggregateEvent).IsAssignableFrom(t))
                .ToArray();

            services
                .AddScoped<IDocumentSerializer>(s => new JsonDocumentSerializer(shortHandTypes))
                .AddScoped<IAggregateFactory, ReflectionConventionAggregateFactory>();

            _services = services;
        }
        
        public EventStoreOptionsBuilder AddDbContext(Action<DbContextOptionsBuilder> optionsAction)
        {
            _services.AddDbContext<EventStoreContext>(optionsAction);
            return this;
        }
        
        public IServiceCollection Services()
        {
            return _services;
        }
    }
}
