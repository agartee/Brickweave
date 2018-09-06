using System;

namespace Brickweave.Cqrs.Cli.Exceptions
{
    public class ExecutableNotFoundException : Exception
    {
        private const string MESSAGE = "No command or query was found with the name, \"{0}\"";

        public ExecutableNotFoundException(string typeShortName)
            :base(string.Format(MESSAGE, typeShortName))
        {
            TypeShortName = typeShortName;
        }

        public string TypeShortName { get; }
    }
}
