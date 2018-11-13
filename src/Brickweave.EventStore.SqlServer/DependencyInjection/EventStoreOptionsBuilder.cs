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
                .Where(t => typeof(IEvent).IsAssignableFrom(t))
                .ToArray();

            SystemDefaults.DocumentSerializer = new JsonDocumentSerializer(shortHandTypes);
            SystemDefaults.AggregateFactory = new ReflectionConventionAggregateFactory();
            
            _services = services;
        }
        
        public EventStoreOptionsBuilder AddDbContext(Action<DbContextOptionsBuilder> optionsAction)
        {
            _services.AddDbContext<EventStoreDbContext>(optionsAction);
            return this;
        }

        public EventStoreOptionsBuilder AddDocumentSerializer(Func<IServiceProvider, IDocumentSerializer> implementationFactory)
        {
            _services.AddScoped(implementationFactory);
            return this;
        }

        public EventStoreOptionsBuilder AddAggregateFactory(Func<IServiceProvider, IAggregateFactory> implementationFactory)
        {
            _services.AddScoped(implementationFactory);
            return this;
        }

        public IServiceCollection Services()
        {
            return _services;
        }
    }
}
