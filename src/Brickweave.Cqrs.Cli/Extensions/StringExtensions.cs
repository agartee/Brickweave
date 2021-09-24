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
                .Select(s => s.Replace("\\\"", "\""))
                .ToArray();
        }

        public static string[] Wrap(this string str, int max)
        {
            var charCount = 0;
            var words = str
                .Replace("\n", " ")
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            return words
                .GroupBy(w => (charCount += (((charCount % max) + w.Length + 1 >= max)
                    ? max - (charCount % max)
                    : 0)+ w.Length + 1) / max)
                .Select(g => string.Join(" ", g.ToArray()))
                .ToArray();
        }

        public static string FirstCharToLowerCase(this string str)
        {
            if (string.IsNullOrEmpty(str) || char.IsLower(str[0]))
                return str;

            return char.ToLower(str[0]) + str.Substring(1);
        }
    }
}
