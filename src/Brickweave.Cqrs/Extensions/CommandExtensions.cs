using System;
using Brickweave.Cqrs.Attributes;

namespace Brickweave.Cqrs.Extensions
{
    public static class CommandExtensions
    {
        public static bool IsLongRunning(this ICommand executable)
        {
            return Attribute.IsDefined(executable.GetType(), typeof(LongRunningAttribute));
        }
    }
}
