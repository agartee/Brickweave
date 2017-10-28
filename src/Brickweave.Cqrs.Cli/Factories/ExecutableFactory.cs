using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Brickweave.Cqrs.Cli.Exceptions;
using Brickweave.Cqrs.Cli.Extensions;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Factories
{
    public class ExecutableFactory : IExecutableFactory
    {
        private readonly IEnumerable<IParameterValueFactory> _parameterFactories;
        private readonly IEnumerable<Type> _executables;
        
        public ExecutableFactory(IEnumerable<IParameterValueFactory> parameterFactories, 
            IEnumerable<Type> executables)
        {
            _parameterFactories = parameterFactories;
            _executables = executables;
        }

        public IExecutable Create(ExecutableInfo executableInfo)
        {
            var executableType = _executables
                .SingleOrDefault(t => t.Name == executableInfo.Name);

            if (executableType == null)
                throw new TypeNotFoundException(executableInfo.Name);

            return Create(executableType, executableInfo.Parameters);
        }

        public IExecutable Create(Type type, Dictionary<string, string> parameterValues)
        {
            var constructor = type.GetConstructors()
                .FirstOrDefault(c => c.ContainsAllParameters(parameterValues.Keys));
            
            if(constructor == null)
                throw new ConstructorNotFoundException(type, parameterValues.Keys.ToArray());

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