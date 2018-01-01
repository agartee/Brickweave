using System;
using System.Linq;

namespace Brickweave.Cqrs.Cli.Extensions
{
    public static class StringExtensions
    {
        public static string UppercaseFirst(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static string[] SplitOnSpacesWithQuotes(this string s)
        {
            return s.Split('"')
                .Select((element, index) => index % 2 == 0
                    ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    : new [] { element })
                .SelectMany(element => element).ToArray();
        }
    }
}
