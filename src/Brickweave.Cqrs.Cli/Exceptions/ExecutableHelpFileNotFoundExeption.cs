using System;

namespace Brickweave.Cqrs.Cli.Exceptions
{
    public class ExecutableHelpFileNotFoundExeption : Exception
    {
        private const string MESSAGE = "XML documentation file not found: {0}.";

        public ExecutableHelpFileNotFoundExeption(string filePath) : base(string.Format(MESSAGE, filePath))
        {
        }
    }
}
