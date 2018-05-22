using System;
using Brickweave.Cqrs.Cli.Extensions;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Factories.ParameterValues
{
    public class BasicParameterValueFactory : ISingleParameterValueFactory
    {
        public bool Qualifies(Type targetType)
        {
            return targetType.IsBasicType();
        }

        public object Create(Type targetType, ExecutableParameterInfo parameter)
        {
            try
            {
                return targetType == typeof(object)
                    ? parameter.SingleValue
                    : ChangeType(parameter.SingleValue, targetType);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Unable to convert {parameter.SingleValue} to type {targetType}", ex);
            }
        }
        
        private static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                    return null;

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }
    }
}