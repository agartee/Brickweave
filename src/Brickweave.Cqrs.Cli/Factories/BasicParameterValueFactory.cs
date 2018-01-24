using System;
using Brickweave.Cqrs.Cli.Extensions;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Factories
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
                    : Convert.ChangeType(parameter.SingleValue, targetType);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Unable to convert {parameter.SingleValue} to type {targetType}", ex);
            }
        }
    }
}