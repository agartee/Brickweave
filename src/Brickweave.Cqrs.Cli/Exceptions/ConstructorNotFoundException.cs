using System;
using System.Collections.Generic;
using System.Linq;

namespace Brickweave.Cqrs.Cli.Exceptions
{
    public class ConstructorNotFoundException : Exception
    {
        private const string MESSAGE = "Constructor not found for type, \"{0}\" containing parameters: {1}";

        public ConstructorNotFoundException(Type type, string[] parameterNames)
            : base(string.Format(MESSAGE, type, string.Join(", ", parameterNames.Select(p => $"\"{p}\""))))
        {
            Type = type;
            Parameters = parameterNames;
        }

        public Type Type { get; }
        public IEnumerable<string> Parameters { get; }
    }
}
