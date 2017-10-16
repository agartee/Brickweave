using System;
using Brickweave.Cqrs.Cli.Extensions;

namespace Brickweave.Cqrs.Cli.Factories
{
    public class BasicParameterValueFactory : IParameterValueFactory
    {
        public bool Qualifies(Type targetType)
        {
            return targetType.IsBasicType();
        }

        public object Create(Type targetType, object parameterValue)
        {
            try
            {
                return targetType == typeof(object)
                    ? parameterValue
                    : Convert.ChangeType(parameterValue, targetType);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Unable to convert {parameterValue} to type {targetType}", ex);
            }
        }
    }
}