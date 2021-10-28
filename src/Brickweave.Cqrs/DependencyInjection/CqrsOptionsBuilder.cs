using System.Reflection;
using Brickweave.Cqrs.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.Cqrs.DependencyInjection
{
    public class CqrsOptionsBuilder
    {
        private readonly IServiceCollection _services;

        public CqrsOptionsBuilder(IServiceCollection services, params Assembly[] domainAssemblies)
        {
            services
                .AddScoped<IDispatcher, Dispatcher>()
                .AddScoped<ICommandDispatcher, CommandDispatcher>()
                .AddScoped<IQueryDispatcher, QueryDispatcher>()
                .AddScoped<ICommandQueue, NullCommandQueue>()
                .AddHandlers(typeof(ICommandHandler<>), domainAssemblies)
                .AddHandlers(typeof(ICommandHandler<,>), domainAssemblies)
                .AddHandlers(typeof(ISecuredCommandHandler<>), domainAssemblies)
                .AddHandlers(typeof(ISecuredCommandHandler<,>), domainAssemblies)
                .AddHandlers(typeof(IQueryHandler<,>), domainAssemblies)
                .AddHandlers(typeof(ISecuredQueryHandler<,>), domainAssemblies);

            _services = services;
        }

        public IServiceCollection Services()
        {
            return _services;
        }
    }
}
