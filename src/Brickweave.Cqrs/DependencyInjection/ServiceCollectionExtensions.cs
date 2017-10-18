using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.Cqrs.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCqrs(this IServiceCollection services, params Assembly[] domainAssemblies)
        {
            return services
                .AddScoped<ICommandExecutor, CommandExecutor>()
                .AddScoped<IQueryExecutor, QueryExecutor>()
                .AddHandlers(typeof(ICommandHandler<>), domainAssemblies)
                .AddHandlers(typeof(ICommandHandler<,>), domainAssemblies)
                .AddHandlers(typeof(ISecuredCommandHandler<>), domainAssemblies)
                .AddHandlers(typeof(ISecuredCommandHandler<,>), domainAssemblies)
                .AddHandlers(typeof(IQueryHandler<,>), domainAssemblies)
                .AddHandlers(typeof(ISecuredQueryHandler<,>), domainAssemblies);
        }

        public static IServiceCollection AddCqrsExecutors(
            this IServiceCollection services, IServiceProvider domainServiceProvider)
        {
            services.AddScoped(provider => domainServiceProvider.GetService<ICommandExecutor>());
            services.AddScoped(provider => domainServiceProvider.GetService<IQueryExecutor>());

            return services;
        }

        private static IServiceCollection AddHandlers(
            this IServiceCollection services, Type handlerType, params Assembly[] assemblies)
        {
            var handlers = assemblies.SelectMany(a => a.ExportedTypes)
                .Where(t => t.IsAssignableToGenericType(handlerType))
                .ToList();

            foreach (var handler in handlers)
            {
                services.AddScoped(
                    handler.GetHandlerInterfaceType(handlerType), handler);
            }

            return services;
        }
    }
}
