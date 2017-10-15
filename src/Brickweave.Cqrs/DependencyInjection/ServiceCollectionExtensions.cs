using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.Cqrs.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommandHandlers(
            this IServiceCollection services, params Assembly[] assemblies)
        {
            return services.AddHandlers(typeof(ICommandHandler<,>), assemblies);
        }

        public static IServiceCollection AddQueryHandlers(
            this IServiceCollection services, params Assembly[] assemblies)
        {
            return services.AddHandlers(typeof(IQueryHandler<,>), assemblies);
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

        public static IServiceCollection AddCommandExecutor(
            this IServiceCollection services)
        {
            services.AddScoped<ICommandExecutor, CommandExecutor>();
            return services;
        }

        public static IServiceCollection AddQueryExecutor(
            this IServiceCollection services)
        {
            services.AddScoped<IQueryExecutor, QueryExecutor>();
            return services;
        }
        
        public static IServiceCollection IsolateDomainServices(
            this IServiceCollection services, IServiceProvider domainServiceProvider)
        {
            services.AddScoped(provider => domainServiceProvider.GetService<ICommandExecutor>());
            services.AddScoped(provider => domainServiceProvider.GetService<IQueryExecutor>());

            return services;
        }
    }
}
