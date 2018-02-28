using System.Collections.Generic;
using System.Linq;
using Brickweave.Cqrs.Cli.Extensions;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Factories
{
    public class NamingConventionExecutableInfoFactory : IExecutableInfoFactory
    {
        public ExecutableInfo Create(string[] args)
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

            IEnumerable<ExecutableParameterInfo> GetParameters()
            {
                var parameters = new List<ExecutableParameterInfo>();
                for (var i = firstParamIndex; i < args.Length; i++)
                {
                    var arg = args[i];

                    if (!arg.StartsWith("-"))
                        continue;

                    var paramName = arg.Substring(2);

                    var paramValues = ParameterHasValue(i)
                        ? GetParamValues(i)
                        : GetArglessParameterDefaultValue();

                    parameters.Add(new ExecutableParameterInfo(paramName, paramValues));
                    i++;
                }

                return parameters;

                bool ParameterHasValue(int i)
                {
                    return args.Length > i + 1 && !args[i + 1].StartsWith("-");
                }

                string[] GetParamValues(int i)
                {
                    var results = new List<string>();

                    for (int j = i; j < args.Length; j++)
                    {
                        if (!args[j].StartsWith("-"))
                            results.Add(args[j]);
                    }

                    return results.ToArray();
                }

                string[] GetArglessParameterDefaultValue()
                {
                    return new [] { "true" };
                }
            }
        }
    }
}
