using System;
using System.Collections.Generic;
using System.Linq;

namespace Brickweave.Cqrs.Cli.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsDefaultable(this Type type)
        {
            return type.IsBasicType() || type.IsGuidType() || type.IsDateTimeType();
        }

        public static bool IsBasicType(this Type type)
        {
            return type == typeof(short) || type == typeof(short?)
                   || type == typeof(int) || type == typeof(int?)
                   || type == typeof(long) || type == typeof(long?)
                   || type == typeof(ulong) || type == typeof(ulong?)
                   || type == typeof(decimal) || type == typeof(decimal?)
                   || type == typeof(float) || type == typeof(float?)
                   || type == typeof(double) || type == typeof(double?)
                   || type == typeof(char) || type == typeof(char?)
                   || type == typeof(bool) || type == typeof(bool?)
                   || type == typeof(string);
        }

        public static bool IsGuidType(this Type type)
        {
            return type == typeof(Guid) || type == typeof(Guid?);
        }

        public static bool IsDateTimeType(this Type type)
        {
            return type == typeof(DateTime) || type == typeof(DateTime?);
        }

        public static bool IsDictionary(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>)
                || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        }

        public static bool HasConstructorWithParameters(this Type type)
        {
            return type.GetConstructors().Any(c => c.GetParameters().Any());
        }
    }
}
