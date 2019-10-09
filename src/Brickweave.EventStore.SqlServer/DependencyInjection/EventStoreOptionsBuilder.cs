using Brickweave.EventStore.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.EventStore.SqlServer.DependencyInjection
{
    public class EventStoreOptionsBuilder
    {
        private readonly IServiceCollection _services;
        
        public EventStoreOptionsBuilder(IServiceCollection services)
        {
            services.AddScoped<IAggregateFactory>(s => new ReflectionConventionAggregateFactory());

            _services = services;
        }

        public IServiceCollection Services()
        {
            return _services;
        }
    }
}
