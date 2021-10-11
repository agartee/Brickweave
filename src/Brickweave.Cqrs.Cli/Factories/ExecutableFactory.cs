using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Brickweave.Cqrs.Cli.Exceptions;
using Brickweave.Cqrs.Cli.Extensions;
using Brickweave.Cqrs.Cli.Factories.ParameterValues;
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
                throw new ExecutableNotFoundException(executableInfo.Name);

            return Create(executableType, executableInfo.Parameters.ToArray());
        }

        public IExecutable Create(Type type, params ExecutableParameterInfo[] parameters)
        {
            var parameterNames = parameters.Select(p => p.Name).ToArray();

            var constructor = type.GetConstructors()
                .FirstOrDefault(c => c.ContainsAllParameters(parameterNames));

            if (constructor == null && type.HasConstructorWithParameters())
                throw new ConstructorNotFoundException(type, parameterNames.ToArray());

            if (constructor != null)
                return CreateViaConstructor(parameters, constructor);
            else
                return CreateViaProperties(parameters, type);
        }

        private IExecutable CreateViaConstructor(ExecutableParameterInfo[] parameters, ConstructorInfo constructor)
        {
            var constructorArgs = GetConstructorParameterValues(constructor, parameters).ToArray();

            return (IExecutable) constructor.Invoke(constructorArgs);
        }

        private IExecutable CreateViaProperties(ExecutableParameterInfo[] parameters, Type type)
        {
            var executable = Activator.CreateInstance(type);
            var typeProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var firstParameterMismatch = parameters
                .FirstOrDefault(p => !typeProperties
                    .Any(tp => tp.Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase)));

            if (firstParameterMismatch != null)
                throw new PropertyNotFoundException(type, firstParameterMismatch.Name);

            foreach (var property in typeProperties)
            {
                var value = GetParameterValue(property, parameters);

                if (!property.PropertyType.IsDefaultable() && value == null && Attribute.IsDefined(property, typeof(RequiredAttribute)))
                    throw new ArgumentNullException(property.Name);

                property.SetValue(executable, value);
            }

            return (IExecutable) executable;
        }

        private IEnumerable<object> GetConstructorParameterValues(ConstructorInfo constructorInfo,
            IEnumerable<ExecutableParameterInfo> parameters)
        {
            return constructorInfo.GetParameters()
                .Select(p => GetParameterValue(p, parameters))
                .ToList();
        }

        private object GetParameterValue(ParameterInfo constructorArg, IEnumerable<ExecutableParameterInfo> parameters)
        {
            var constructorParam = parameters
                .SingleOrDefault(p => constructorArg.Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase));

            if (constructorArg.ParameterType.IsDefaultable())
            {
                if (string.IsNullOrWhiteSpace(constructorParam?.SingleValue))
                    return constructorArg.DefaultValue != DBNull.Value ? constructorArg.DefaultValue : null;
            }

            var paramFactory = _parameterValueFactories.ToList()
                .FirstOrDefault(f => f.Qualifies(constructorArg.ParameterType));

            if (paramFactory == null)
                throw new NoQualifyingParameterValueFactoryException(constructorArg.ParameterType.Name);

            return paramFactory.Create(constructorArg.ParameterType, constructorParam);
        }

        private object GetParameterValue(PropertyInfo property, IEnumerable<ExecutableParameterInfo> parameters)
        {
            var propertyParam = parameters
                .SingleOrDefault(p => property.Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase));

            var paramFactory = _parameterValueFactories.ToList()
                .FirstOrDefault(f => f.Qualifies(property.PropertyType));

            if (paramFactory == null)
                throw new NoQualifyingParameterValueFactoryException(property.PropertyType.Name);

            return paramFactory.Create(property.PropertyType, propertyParam);
        }
    }
}
