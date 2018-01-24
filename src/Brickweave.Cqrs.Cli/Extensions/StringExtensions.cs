using System.Collections.Generic;
using Brickweave.Cqrs.Cli.Factories;

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

        public static string[] ParseExecutableString(this string s)
        {
            var results = new List<string>();

            var current = string.Empty;
            var isInQuoteBlock = false;
            var isInMultiValueBlock = false;

            for (var i = 0; i < s.Length; i++)
            {
                var character = s[i];

                switch (character)
                {
                    case ' ':
                        if (isInQuoteBlock)
                        {
                            current += character;
                            break;
                        }
                        
                        if (isInMultiValueBlock)
                        {
                            for (var j = i + 1; j < s.Length; j++)
                            {
                                if (s[j] == '-')
                                {
                                    isInMultiValueBlock = false;
                                    results.Add(current);
                                    current = string.Empty;
                                    break;
                                }

                                if (s[j] != ' ')
                                {
                                    break;
                                }
                            }
                        }

                        else
                        {
                            results.Add(current);
                            current = string.Empty;
                        }
                        
                        break;
                    case '"':
                        isInQuoteBlock = !isInQuoteBlock;
                        break;
                    case ',':
                        if (isInQuoteBlock)
                        {
                            current += character;
                            break;
                        }

                        if (isInMultiValueBlock)
                        {
                            current += MultiParameterValueSeparator.Default;
                            break;
                        }

                        isInMultiValueBlock = true;
                        current += MultiParameterValueSeparator.Default;
                        break;
                    default:
                        current += character;
                        break;
                }
            }

            results.Add(current);

            return results.ToArray();
        }
    }
}
