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
        private readonly IEnumerable<IParameterValueFactory> _parameterValueFactories;
        private readonly IEnumerable<Type> _executables;
        
        public ExecutableFactory(IEnumerable<IParameterValueFactory> parameterValueFactories, 
            IEnumerable<Type> executables)
        {
            _parameterValueFactories = parameterValueFactories;
            _executables = executables;
        }

        public bool Exists(string name)
        {
            return _executables.Any(t => t.Name == name);
        }

        public IExecutable Create(ExecutableInfo executableInfo)
        {
            var executableType = _executables
                .SingleOrDefault(t => t.Name == executableInfo.Name);

            if (executableType == null)
                throw new TypeNotFoundException(executableInfo.Name);

            return Create(executableType, executableInfo.Parameters.ToArray());
        }

        public IExecutable Create(Type type, params ExecutableParameterInfo[] parameterValues)
        {
            var parameterNames = parameterValues.Select(p => p.Name).ToArray();

            var constructor = type.GetConstructors()
                .FirstOrDefault(c => c.ContainsAllParameters(parameterNames));
            
            if(constructor == null)
                throw new ConstructorNotFoundException(type, parameterNames.ToArray());

            var constructorArgs = GetConstructorParameterValues(constructor).ToArray();

            return (IExecutable)constructor.Invoke(constructorArgs);
            
            IEnumerable<object> GetConstructorParameterValues(ConstructorInfo constructorInfo)
            {
                return constructorInfo.GetParameters()
                    .Select(GetParameterValue)
                    .ToList();

                object GetParameterValue(ParameterInfo constructorParam)
                {
                    var constructorArgValue = parameterValues
                        .SingleOrDefault(p => IsValueForParameter(p.Name));

                    if (constructorParam.ParameterType.IsDefaultable())
                    {
                        if (string.IsNullOrWhiteSpace(constructorArgValue?.SingleValue))
                            return constructorParam.DefaultValue != DBNull.Value ? constructorParam.DefaultValue : null;
                    }

                    var paramFactory = _parameterValueFactories.ToList()
                        .FirstOrDefault(f => f.Qualifies(constructorParam.ParameterType));

                    if (paramFactory == null)
                        throw new NoQualifyingParameterValueFactoryException(constructorParam.ParameterType.Name);

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