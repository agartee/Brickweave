using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brickweave.Cqrs.Cli.Exceptions;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Factories.ParameterValues
{
    public class EnumerableParameterValueFactory : IParameterValueFactory
    {
        private readonly IEnumerable<IParameterValueFactory> _parameterValueFactories;

        public EnumerableParameterValueFactory(IEnumerable<IParameterValueFactory> parameterValueFactories)
        {
            _parameterValueFactories = parameterValueFactories;
        }

        public bool Qualifies(Type targetType)
        {
            return typeof(IEnumerable).IsAssignableFrom(targetType) && targetType.GetGenericArguments().Any() 
                && targetType.IsGenericType && targetType.GetGenericTypeDefinition() != typeof(IList<>) 
                && targetType.IsGenericType && targetType.GetGenericTypeDefinition() != typeof(List<>);
        }
        
        public object Create(Type targetType, ExecutableParameterInfo parameter)
        {
            var genericType = targetType.GetGenericArguments().First();
            var paramFactory = _parameterValueFactories.ToList()
                .FirstOrDefault(f => f.Qualifies(genericType));

            if (paramFactory == null)
                throw new NoQualifyingParameterValueFactoryException(genericType.Name);

            var paramValues = parameter.Values
                .Select((v, i) => new
                {
                    Index = i,
                    Value = paramFactory.Create(genericType, new ExecutableParameterInfo(string.Empty, v))
                }).ToList();
            
            var result = Array.CreateInstance(genericType, paramValues.Count);

            paramValues.ForEach(pv => result.SetValue(pv.Value, pv.Index));

            return result;
        }
    }
}