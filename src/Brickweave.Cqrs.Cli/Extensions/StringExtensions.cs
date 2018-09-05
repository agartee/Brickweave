using System;
using System.Linq;
using System.Text.RegularExpressions;

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

        public static string[] ParseCommandText(this string str)
        {
            return new Regex(@"[^\s""]+|""[^""\\]*(?:\\.[^""\\]*)*""")
                .Matches(str).OfType<Match>()
                .Select(m => m.Value.Trim(new[] { '"' }))
                .ToArray();
        }

        public static string[] Wrap(this string str, int max)
        {
            var charCount = 0;
            var lines = str.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            return lines
                .GroupBy(w => (charCount += (((charCount % max) + w.Length + 1 >= max)
                    ? max - (charCount % max)
                    : 0)+ w.Length + 1) / max)
                .Select(g => string.Join(" ", g.ToArray()))
                .ToArray();
        }

        private static string RemoveFirstAndLast(this string str)
        {
            return string.Join(string.Empty, str.Skip(1).Take(str.Length - 2));
        }
    }
}
