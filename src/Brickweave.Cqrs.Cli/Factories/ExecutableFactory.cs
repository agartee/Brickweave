using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Brickweave.Cqrs.Cli.Exceptions;
using Brickweave.Cqrs.Cli.Models;
using Brickweave.Cqrs;

namespace Brickweave.Cqrs.Cli.Factories
{
    public class ExecutableFactory : IExecutableFactory
    {
        private readonly IEnumerable<IParameterValueFactory> _parameterFactories;
        private readonly IEnumerable<Assembly> _assemblies;

        public ExecutableFactory(IEnumerable<IParameterValueFactory> parameterFactories, 
            IEnumerable<Assembly> assemblies)
        {
            _assemblies = assemblies;
            _parameterFactories = parameterFactories;
        }

        public IExecutable Create(ExecutableInfo executableInfo)
        {
            var executableType = _assemblies
                .SelectMany(a => a.ExportedTypes)
                .Where(t => typeof(IExecutable).IsAssignableFrom(t.GetTypeInfo()))
                .SingleOrDefault(t => t.Name == executableInfo.Name);

            if (executableType == null)
                throw new TypeNotFoundException(executableInfo.Name);

            return Create(executableType, executableInfo.Parameters);
        }

        public IExecutable Create(Type type, Dictionary<string, string> parameterValues)
        {
            var constructor = type.GetConstructors().First();
            
            return (IExecutable)constructor.Invoke(
                GetConstructorParameterValues(constructor).ToArray());

            IEnumerable<object> GetConstructorParameterValues(ConstructorInfo constructorInfo)
            {
                return constructorInfo.GetParameters()
                    .Select(GetParameterValue)
                    .ToList();

                object GetParameterValue(ParameterInfo constructorParam)
                {
                    var constructorArgValue = parameterValues
                        .Where(kvp => IsValueForParameter(kvp.Key))
                        .Select(kvp => kvp.Value)
                        .SingleOrDefault();

                    if (string.IsNullOrWhiteSpace(constructorArgValue))
                        return constructorParam.DefaultValue != DBNull.Value ? constructorParam.DefaultValue : null;

                    var paramFactory = _parameterFactories.ToList()
                        .First(f => f.Qualifies(constructorParam.ParameterType));

                    return paramFactory.Create(constructorParam.ParameterType, constructorArgValue);

                    bool IsValueForParameter(string parameterName)
                    {
                        var result = type.GetProperties()
                            .Any(propertyInfo => IsMatchBasedOnName());

                        return result;

                        bool IsMatchBasedOnName()
                        {
                            return parameterName.Equals(constructorParam.Name,
                                StringComparison.OrdinalIgnoreCase);
                        }
                    }
                }
            }
        }
    }
}