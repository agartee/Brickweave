using System;

namespace Brickweave.Cqrs.Cli.Exceptions
{
    public class CategoryHelpFileNotFoundExeption : Exception
    {
        private const string MESSAGE = "Subject file not found.";

        public CategoryHelpFileNotFoundExeption() : base(MESSAGE)
        {
        }
    }
}
