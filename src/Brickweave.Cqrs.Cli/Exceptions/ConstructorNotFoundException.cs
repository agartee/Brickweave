using System;
using System.Collections.Generic;
using System.Linq;

namespace Brickweave.Cqrs.Cli.Exceptions
{
    public class ConstructorNotFoundException : Exception
    {
        private const string MESSAGE = "Constructor not found for type, \"{0}\" containing parameters: {1}";

        public ConstructorNotFoundException(Type type, string[] parameters)
            : base(string.Format(MESSAGE, type, string.Join(", ", parameters.Select(p => $"\"{p}\""))))
        {
            Type = type;
            Parameters = parameters;
        }

        public Type Type { get; }
        public IEnumerable<string> Parameters { get; }
    }
}