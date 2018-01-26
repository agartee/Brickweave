using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brickweave.Cqrs.Cli.Exceptions;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Factories.ParameterValues
{
    public class ListParameterValueFactory : IParameterValueFactory
    {
        private readonly IEnumerable<IParameterValueFactory> _parameterValueFactories;

        public ListParameterValueFactory(IEnumerable<IParameterValueFactory> parameterValueFactories)
        {
            _parameterValueFactories = parameterValueFactories;
        }

        public bool Qualifies(Type targetType)
        {
            return targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(IList<>) 
                || targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(List<>);
        }

        public object Create(Type targetType, ExecutableParameterInfo parameter)
        {
            var genericType = targetType.GetGenericArguments().First();
            var paramFactory = _parameterValueFactories.ToList()
                .FirstOrDefault(f => f.Qualifies(genericType));

            if (paramFactory == null)
                throw new NoQualifyingParameterValueFactoryException(genericType.Name);

            var result = (IList) Activator.CreateInstance(typeof(List<>).MakeGenericType(genericType));

            parameter.Values
                .Select(v => paramFactory.Create(genericType, new ExecutableParameterInfo(string.Empty, v)))
                .ToList().ForEach(v => result.Add(v));
            
            return result;
        }
    }
}