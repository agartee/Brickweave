using System;

namespace Brickweave.Cqrs.Cli.Exceptions
{
    public class PropertyNotFoundException : Exception
    {
        private const string MESSAGE = "Property not found for type, \"{0}\": {1}";

        public PropertyNotFoundException(Type type, string propertyName)
            : base(string.Format(MESSAGE, type, string.Join(", ", propertyName)))
        {
            Type = type;
            Parameter = propertyName;
        }

        public Type Type { get; }
        public string Parameter { get; }
    }
}
