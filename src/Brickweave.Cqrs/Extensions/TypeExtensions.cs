using System;
using System.Linq;
using System.Reflection;

namespace Brickweave.Cqrs.Extensions
{
    public static class TypeExtensions
    {
        public static Type GetCommandReturnType(this object instance)
        {
            var type = instance.GetType();
            
            var result = type.GetTypeInfo().ImplementedInterfaces
                .Where(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommand<>))
                .Select(i => i.GenericTypeArguments[0])
                .SingleOrDefault();

            return result;
        }

        public static Type GetQueryReturnType(this object instance)
        {
            var type = instance.GetType();
            
            var result = type.GetTypeInfo().ImplementedInterfaces
                .Where(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IQuery<>))
                .Select(i => i.GenericTypeArguments[0])
                .SingleOrDefault();

            return result;
        }
    }
}
