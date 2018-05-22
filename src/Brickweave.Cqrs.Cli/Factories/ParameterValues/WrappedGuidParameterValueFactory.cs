using System;
using System.Linq;
using System.Reflection;
using Brickweave.Cqrs.Cli.Extensions;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Factories.ParameterValues
{
    public class WrappedGuidParameterValueFactory : ISingleParameterValueFactory
    {
        public bool Qualifies(Type targetType)
        {
            return GetWrappedGuidConstructor(targetType) != null;
        }

        public object Create(Type targetType, ExecutableParameterInfo parameter)
        {
            if (parameter == null)
                return null;

            if (parameter.SingleValue.GetType() != typeof(string))
                throw new InvalidOperationException($"Unsupported type for {typeof(IExecutable)}:{targetType} parameter conversion: {parameter.SingleValue.GetType()}");

            var constructor = GetWrappedGuidConstructor(targetType);

            return constructor.Invoke(new[] { (object) new Guid(parameter.SingleValue) });
        }

        private static ConstructorInfo GetWrappedGuidConstructor(Type type)
        {
            return type.GetConstructors()
                .Where(c => c.GetParameters().Length == 1)
                .FirstOrDefault(c => c.GetParameters().First().ParameterType.IsGuidType());
        }
    }
}