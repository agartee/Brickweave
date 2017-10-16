using System;
using System.Linq;
using System.Reflection;
using Brickweave.Cqrs.Cli.Extensions;

namespace Brickweave.Cqrs.Cli.Factories
{
    public class WrappedBasicParameterValueFactory : IParameterValueFactory
    {
        public bool Qualifies(Type targetType)
        {
            return IsWrappedBasicType(targetType);
        }

        public object Create(Type targetType, object parameterValue)
        {
            var constructor = GetWrappedBasicConstructor(targetType);
            var parameterType = GetWrappedBasicType(targetType);

            return constructor.Invoke(new [] { Convert.ChangeType(parameterValue, parameterType) });
        }

        private static ConstructorInfo GetWrappedBasicConstructor(Type type)
        {
            return type.GetConstructors()
                .Where(c => c.GetParameters().Length == 1)
                .FirstOrDefault(c => c.GetParameters().First().ParameterType.IsBasicType());
        }

        private static Type GetWrappedBasicType(Type type)
        {
            return GetWrappedBasicConstructor(type)?.GetParameters()
                .First().ParameterType;
        }
        
        private static bool IsWrappedBasicType(Type type)
        {
            return GetWrappedBasicType(type) != null;
        }
    }
}
