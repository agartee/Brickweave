using System;

namespace Brickweave.Cqrs.Cli.Exceptions
{
    public class TypeNotFoundException : Exception
    {
        private const string MESSAGE = "Type not found: \"{0}\"";

        public TypeNotFoundException(string typeShortName)
            :base(string.Format(MESSAGE, typeShortName))
        {
            TypeShortName = typeShortName;
        }

        public string TypeShortName { get; }
    }
}
