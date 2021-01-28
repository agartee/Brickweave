using System;
using Brickweave.Cqrs.Attributes;

namespace Brickweave.Cqrs.Extensions
{
    public static class CommandExtensions
    {
        public static bool IsLongRunning(this ICommand command)
        {
            var attribute = Attribute.GetCustomAttribute(command.GetType(), typeof(LongRunningCommandAttribute));
            return attribute != null;
        }
    }
}
