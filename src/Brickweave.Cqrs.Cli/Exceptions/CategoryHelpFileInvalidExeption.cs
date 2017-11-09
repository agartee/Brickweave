using System;

namespace Brickweave.Cqrs.Cli.Exceptions
{
    public class CategoryHelpFileInvalidExeption : Exception
    {
        private const string MESSAGE = "Subject help file is incorrectly formatted.";

        public CategoryHelpFileInvalidExeption() : base(MESSAGE)
        {
        }
    }
}