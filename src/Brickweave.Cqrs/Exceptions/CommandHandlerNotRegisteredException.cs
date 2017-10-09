using System;

namespace Brickweave.Cqrs.Exceptions
{
    public class CommandHandlerNotRegisteredException : Exception
    {
        private const string MESSAGE = "Handler not registered for command, {0}";

        public CommandHandlerNotRegisteredException(ICommand command)
            : base(string.Format(MESSAGE, command.GetType()))
        {
            Command = command;
        }

        public ICommand Command { get; }
    }
}
