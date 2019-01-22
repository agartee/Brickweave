using System;
using Brickweave.Cqrs.Cli.Extensions;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Factories.ParameterValues
{
    public class BasicParameterValueFactory : ISingleParameterValueFactory
    {
        public bool Qualifies(Type targetType)
        {
            return targetType.IsBasicType() || targetType == typeof(object);
        }

        public object Create(Type targetType, ExecutableParameterInfo parameter)
        {
            try
            {
                return targetType == typeof(object)
                    ? parameter.SingleValue.AutoCorrectType()
                    : ChangeType(parameter.SingleValue, targetType);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Unable to convert {parameter.SingleValue} to type {targetType}", ex);
            }
        }
        
        private static object ChangeType(object value, Type targetType)
        {
            var type = targetType;

            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                    return null;

                type = Nullable.GetUnderlyingType(type);
            }

            return Convert.ChangeType(value, type);
        }
    }
}