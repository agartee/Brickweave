using System;
using System.Collections.Generic;
using System.Linq;
using Brickweave.Cqrs.Cli.Extensions;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Factories
{
    public class ExecutableInfoFactory : IExecutableInfoFactory
    {
        private readonly IEnumerable<IExecutableRegistration> _executableRegistrations;
        
        public ExecutableInfoFactory(params IExecutableRegistration[] executableRegistrations)
        {
            _executableRegistrations = executableRegistrations;
        }

        public ExecutableInfo Create(string[] args)
        {
            var firstParamIndex = GetFirstParamIndex(args);
            var name = GetNameByRegistration(args, firstParamIndex)
                ?? GetNameByConvention(args, firstParamIndex);
            var parameters = GetParameters(args, firstParamIndex);

            return new ExecutableInfo(name, parameters);
        }

        private int GetFirstParamIndex(string[] args)
        {
            var firstParam = args
                .Select((arg, index) => new { Value = arg, Index = index })
                .FirstOrDefault(arg => arg.Value.StartsWith("-"));

            return firstParam?.Index ?? args.Count();
        }

        private string GetNameByRegistration(string[] args, int firstParamIndex)
        {
            var nameParts = args
                .Take(firstParamIndex)
                .Select(p => p.ToLower())
                .ToList();

            var actionName = nameParts[nameParts.Count - 1];
            var subjectName = string.Join(" ", nameParts.Take(nameParts.Count - 1));

            return _executableRegistrations
                .Where(r => r.SubjectName == subjectName)
                .FirstOrDefault(r => r.ActionName == actionName)?
                .Type.Name;
        }

        private string GetNameByConvention(string[] args, int firstParamIndex)
        {
            var nameParts = args
                .Take(firstParamIndex)
                .Select(p => p.UppercaseFirst())
                .ToList();

            var actionName = nameParts[nameParts.Count - 1];
            var subjectNameParts = nameParts.Take(nameParts.Count - 1);

            var orderedNameParts = new List<string> { actionName };
            orderedNameParts.AddRange(subjectNameParts);

            return string.Join("", orderedNameParts);
        }

        private IEnumerable<ExecutableParameterInfo> GetParameters(string[] args, int firstParamIndex)
        {
            var parameters = new List<ExecutableParameterInfo>();
            for (var i = firstParamIndex; i < args.Length; i++)
            {
                var arg = args[i];

                if (!arg.StartsWith("-"))
                    continue;

                var paramName = arg.Substring(2);

                string[] paramValues = null;
                if(ArgHasParameterValue(args, i))
                {
                    paramValues = GetArgParameterValues(args, i);
                    i++;
                }
                else
                {
                    paramValues = GetDefaultArgParameterValue();
                }

                parameters.Add(new ExecutableParameterInfo(paramName, paramValues));
            }

            return parameters;            
        }

        private bool ArgHasParameterValue(string[] args, int i)
        {
            return args.Length > i + 1 && !args[i + 1].StartsWith("-");
        }

        private string[] GetArgParameterValues(string[] args, int i)
        {
            var results = new List<string>();

            for (var j = i + 1; j < args.Length; j++)
            {
                if (args[j].StartsWith("-"))
                    break;

                results.Add(args[j]);
            }

            return results.ToArray();
        }

        private string[] GetDefaultArgParameterValue()
        {
            return new[] { "true" };
        }
    }
}
