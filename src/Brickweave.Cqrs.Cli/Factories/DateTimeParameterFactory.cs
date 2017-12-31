using System;
using System.Globalization;

namespace Brickweave.Cqrs.Cli.Factories
{
    public class DateTimeParameterFactory : IParameterValueFactory
    {
        private readonly CultureInfo _culture;

        public DateTimeParameterFactory(CultureInfo culture)
        {
            _culture = culture;
        }

        public bool Qualifies(Type targetType)
        {
            return targetType == typeof(DateTime)
                || targetType == typeof(DateTime?);
        }

        public object Create(Type targetType, object parameterValue)
        {
            Guard.IsStringOrNull(parameterValue);

            if (targetType == typeof(DateTime?))
            {
                if (string.IsNullOrWhiteSpace((string) parameterValue))
                    return null;
            }

            if (DateTime.TryParse((string) parameterValue,
                _culture, DateTimeStyles.None, out var result))
            {
                return result;
            }

            throw new InvalidOperationException("Invalid DateTime format.");
        }

        private static class Guard
        {
            public static void IsStringOrNull(object parameterValue)
            {
                if(parameterValue != null && !(parameterValue is string))
                    throw new InvalidOperationException($"Unable to parse DateTime. \"{parameterValue}\" is not a string.");
            }
        }
    }
}
