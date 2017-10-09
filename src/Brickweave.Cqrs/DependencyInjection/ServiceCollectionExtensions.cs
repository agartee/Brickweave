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

        public static IServiceCollection AddCommandProcessor(
            this IServiceCollection services)
        {
            services.AddScoped<ICommandProcessor, CommandProcessor>();
            return services;
        }

        public static IServiceCollection AddQueryProcessor(
            this IServiceCollection services)
        {
            services.AddScoped<IQueryProcessor, QueryProcessor>();
            return services;
        }
        
        public static IServiceCollection IsolateDomainServices(
            this IServiceCollection services, IServiceProvider domainServiceProvider)
        {
            services.AddScoped(provider => domainServiceProvider.GetService<ICommandProcessor>());
            services.AddScoped(provider => domainServiceProvider.GetService<IQueryProcessor>());

            return services;
        }
    }
}
