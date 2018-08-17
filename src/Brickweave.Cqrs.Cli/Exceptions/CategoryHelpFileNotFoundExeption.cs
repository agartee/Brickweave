using System;

namespace Brickweave.Cqrs.Cli.Exceptions
{
    public class CategoryHelpFileNotFoundExeption : Exception
    {
        private const string MESSAGE = "Category help file not found.";

        public CategoryHelpFileNotFoundExeption() : base(MESSAGE)
        {
        }
    }
}
