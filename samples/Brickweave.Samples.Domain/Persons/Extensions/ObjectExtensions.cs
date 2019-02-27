using System;

namespace Brickweave.Samples.Domain.Persons.Extensions
{
    public static class ObjectExtensions
    {
        public static object CorrectType(this object obj)
        {
            return obj
                .ToIntIfInt()
                .ToGuidIfGuid();
        }

        private static object ToIntIfInt(this object obj)
        {
            var converted = int.TryParse(obj.ToString(), out int result);

            return converted ? result : obj;
        }

        private static object ToGuidIfGuid(this object obj)
        {
            var converted = Guid.TryParse(obj.ToString(), out Guid result);

            return converted ? result : obj;
        }
    }
}
