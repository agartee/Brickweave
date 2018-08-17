using System;

namespace Brickweave.Cqrs.Cli.Exceptions
{
    public class ExecutableHelpFileInvalidExeption : Exception
    {
        private const string MESSAGE = "Executable help file is incorrectly formatted.";

        public ExecutableHelpFileInvalidExeption() : base(MESSAGE)
        {
        }
    }
}