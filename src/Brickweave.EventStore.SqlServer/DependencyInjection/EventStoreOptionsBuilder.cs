using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Brickweave.EventStore.Factories;
using Brickweave.EventStore.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Brickweave.EventStore.SqlServer.DependencyInjection
{
    public class EventStoreOptionsBuilder
    {
        private readonly IServiceCollection _services;
        private readonly IList<JsonConverter> _converters = new List<JsonConverter>();
        
        public EventStoreOptionsBuilder(IServiceCollection services, params Assembly[] domainAssemblies)
        {
            var shortHandTypes = domainAssemblies.SelectMany(a => a.ExportedTypes)
                .Where(t => typeof(IEvent).IsAssignableFrom(t))
                .ToArray();
            
            services.AddScoped<IDocumentSerializer>(s => new JsonDocumentSerializer(shortHandTypes, _converters.ToArray()));
            services.AddScoped<IAggregateFactory>(s => new ReflectionConventionAggregateFactory());

            _services = services;
        }

        public EventStoreOptionsBuilder AddJsonConverter(JsonConverter converter)
        {
            _converters.Add(converter);
            return this;
        }

        public IServiceCollection Services()
        {
            return _services;
        }
    }
}
