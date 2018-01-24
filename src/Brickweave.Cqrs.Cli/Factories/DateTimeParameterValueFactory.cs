using System;
using System.Globalization;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Factories
{
    public class DateTimeParameterValueFactory : ISingleParameterValueFactory
    {
        private readonly CultureInfo _culture;

        public DateTimeParameterValueFactory(CultureInfo culture)
        {
            _culture = culture;
        }

        public bool Qualifies(Type targetType)
        {
            return targetType == typeof(DateTime)
                || targetType == typeof(DateTime?);
        }

        public object Create(Type targetType, ExecutableParameterInfo parameter)
        {
            if (targetType == typeof(DateTime?))
            {
                if (string.IsNullOrWhiteSpace(parameter.SingleValue))
                    return null;
            }

            if (DateTime.TryParse(parameter.SingleValue,
                _culture, DateTimeStyles.None, out var result))
            {
                return result;
            }

            throw new InvalidOperationException("Invalid DateTime format.");
        }
    }
}
