using System;
using System.Linq;
using System.Reflection;

namespace Brickweave.Cqrs.DependencyInjection
{
    internal static class TypeExtensions
    {
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
