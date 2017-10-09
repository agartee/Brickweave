using System;

namespace Brickweave.Core.Exceptions
{
    public class GuardException : Exception
    {
        public GuardException(string message) : base(message)
        {
        }
    }
}