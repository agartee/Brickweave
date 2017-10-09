using System;

namespace Brickweave.Cqrs.Exceptions
{
    public class GuardException : Exception
    {
        public GuardException(string message) : base(message)
        {
        }
    }
}