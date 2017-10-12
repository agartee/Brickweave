using System;

namespace Brickweave.EventStore.Exceptions
{
    public class UnrecognizedTypeException : Exception
    {
        private const string MESSAGE = "Unknown type: {0}";

        public UnrecognizedTypeException(string typeName) :
            base(string.Format(MESSAGE, typeName))
        {
            TypeName = typeName;
        }

        public string TypeName { get; }
    }
}