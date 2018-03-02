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

        public static string[] ParseCommandText(this string s)
        {
            return s.Split('"')
                .Select((element, index) => index % 2 == 0
                    ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    : new[] { element })
                .SelectMany(element => element).ToArray();
        }

        public static string[] Wrap(this string text, int max)
        {
            var charCount = 0;
            var lines = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            return lines
                .GroupBy(w => (charCount += (((charCount % max) + w.Length + 1 >= max)
                                                ? max - (charCount % max)
                                                : 0)
                                            + w.Length + 1) / max)
                .Select(g => string.Join(" ", g.ToArray()))
                .ToArray();
        }
    }
}
