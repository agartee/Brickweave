using System;
using System.Linq;
using System.Reflection;
using Brickweave.Cqrs.Cli.Extensions;

namespace Brickweave.Cqrs.Cli.Factories
{
    public class WrappedGuidParameterValueFactory : IParameterValueFactory
    {
        public bool Qualifies(Type targetType)
        {
            return IsWrappedGuidType(targetType);
        }

        public object Create(Type targetType, object parameterValue)
        {
            if (parameterValue.GetType() != typeof(string))
                throw new InvalidOperationException($"Unsupported type for {typeof(IExecutable)}:{targetType} parameter conversion: {parameterValue.GetType()}");

            var constructor = GetWrappedGuidConstructor(targetType);

            return constructor.Invoke(new[] { (object) new Guid((string) parameterValue)});
        }

        private static ConstructorInfo GetWrappedGuidConstructor(Type type)
        {
            return type.GetConstructors()
                .Where(c => c.GetParameters().Length == 1)
                .FirstOrDefault(c => c.GetParameters().First().ParameterType.IsGuidType());
        }

        private static bool IsWrappedGuidType(Type type)
        {
            return GetWrappedGuidConstructor(type) != null;
        }
    }
}