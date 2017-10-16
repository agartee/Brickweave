using System.Collections.Generic;
using System.Linq;
using Brickweave.Cqrs.Cli.Extensions;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Parsers
{
    public class NamingConventionArgParser : IArgParser
    {
        public ExecutableInfo Parse(string[] args)
        {
            var firstParamIndex = GetFirstParamIndex();
            
            return new ExecutableInfo(GetName(), GetParameters());
            
            int GetFirstParamIndex()
            {
                return args
                    .Select((arg, index) => new { Value = arg, Index = index })
                    .First(arg => arg.Value.StartsWith("-"))
                    .Index;
            }

            string GetName()
            {
                var nameParts = args
                    .Take(firstParamIndex)
                    .Select(p => p.UppercaseFirst())
                    .ToList();

                var orderedNameParts = new List<string> { nameParts[nameParts.Count - 1] };
                orderedNameParts.AddRange(nameParts.Take(nameParts.Count - 1));

                return string.Join("", orderedNameParts);
            }

            Dictionary<string, string> GetParameters()
            {
                var parameters = new Dictionary<string, string>();
                for (var i = firstParamIndex; i < args.Length; i++)
                {
                    var arg = args[i];

                    if (!arg.StartsWith("-"))
                        continue;

                    var paramName = arg.Substring(2);
                    var paramValue = ParameterHasValue(i)
                        ? args[i + 1]
                        : GetParameterDefaultValue();

                    parameters.Add(paramName, paramValue);
                    i++;
                }

                return parameters;

                bool ParameterHasValue(int i)
                {
                    return args.Length > i + 1 && !args[i + 1].StartsWith("-");
                }

                string GetParameterDefaultValue()
                {
                    return "true";
                }
            }
        }
    }
}
