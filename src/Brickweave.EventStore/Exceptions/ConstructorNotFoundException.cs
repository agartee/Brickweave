using System;
using System.Linq;

namespace Brickweave.EventStore.Exceptions
{
    public class ConstructorNotFoundException : Exception
    {
        private const string MESSAGE = "Unable to find constructor in {0} with parameter(s): {1}.";

        public ConstructorNotFoundException(Type type, Type[] parameterTypes)
            : base(string.Format(MESSAGE, type, string.Join(", ", parameterTypes.Select(t => t.ToString()))))
        {
            Type = type;
            ParameterTypes = parameterTypes;
        }

        public Type Type { get; }
        public Type[] ParameterTypes { get; }
    }
}
