using System;

namespace Brickweave.Cqrs.Cli.Factories
{
    public class BasicParameterValueFactory : IParameterValueFactory
    {
        public bool Qualifies(Type targetType)
        {
            return targetType == typeof(short) || targetType == typeof(short?)
                || targetType == typeof(int) || targetType == typeof(int?)
                || targetType == typeof(long) || targetType == typeof(long?)
                || targetType == typeof(ulong) || targetType == typeof(ulong?)
                || targetType == typeof(decimal) || targetType == typeof(decimal?)
                || targetType == typeof(float) || targetType == typeof(float?)
                || targetType == typeof(double) || targetType == typeof(double?)
                || targetType == typeof(char) || targetType == typeof(char?)
                || targetType == typeof(bool) || targetType == typeof(bool?)
                || targetType == typeof(DateTime) || targetType == typeof(DateTime?)
                || targetType == typeof(string);
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