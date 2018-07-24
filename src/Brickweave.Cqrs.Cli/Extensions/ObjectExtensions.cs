using System;

namespace Brickweave.Cqrs.Cli.Extensions
{
    public static class ObjectExtensions
    {
        public static object InterpretType(this object obj)
        {
            return obj
                .ToLongIfLong()
                .ToGuidIfGuid();
        }

        private static object ToLongIfLong(this object obj)
        {
            var converted = long.TryParse(obj.ToString(), out long result);

            return converted ? result : obj;
        }

        private static object ToGuidIfGuid(this object obj)
        {
            var converted = Guid.TryParse(obj.ToString(), out Guid result);

            return converted ? result : obj;
        }
    }
}
