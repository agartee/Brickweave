using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.Cqrs.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static CqrsOptionsBuilder AddBrickweaveCqrs(this IServiceCollection services, params Assembly[] domainAssemblies)
        {
            return new CqrsOptionsBuilder(services, domainAssemblies);
        }

        internal static IServiceCollection AddHandlers(this IServiceCollection services, Type handlerType, params Assembly[] assemblies)
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

        internal static Type GetHandlerInterfaceType(this Type implementationType, Type serviceType)
        {
            var result = implementationType.GetTypeInfo().ImplementedInterfaces
                .Where(it => it.GetTypeInfo().IsGenericType)
                .FirstOrDefault(it => it.GetGenericTypeDefinition() == serviceType);

            if (result != null)
                return result;

            var baseType = serviceType.GetTypeInfo().BaseType;
            if (baseType != null)
                return GetHandlerInterfaceType(baseType, serviceType);

            throw new InvalidOperationException(
                $"Type {implementationType} does not implement {serviceType}");
        }

        internal static bool IsAssignableToGenericType(this Type type, Type assignableType)
        {
            if (type.GetTypeInfo().ImplementedInterfaces
                .Where(it => it.GetTypeInfo().IsGenericType)
                .Any(it => it.GetGenericTypeDefinition() == assignableType))
            {
                return true;
            }

            if (type.GetTypeInfo().IsGenericType
                && type.GetGenericTypeDefinition() == assignableType)
            {
                return true;
            }

            var baseType = type.GetTypeInfo().BaseType;

            return baseType != null
                && IsAssignableToGenericType(baseType, assignableType);
        }
    }
}
