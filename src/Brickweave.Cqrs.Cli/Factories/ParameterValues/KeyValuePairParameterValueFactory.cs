using System;
using System.Collections.Generic;
using System.Linq;
using Brickweave.Cqrs.Cli.Exceptions;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Factories.ParameterValues
{
    public class KeyValuePairParameterValueFactory : ISingleParameterValueFactory
    {
        private readonly IEnumerable<ISingleParameterValueFactory> _parameterValueFactories;

        public KeyValuePairParameterValueFactory(IEnumerable<ISingleParameterValueFactory> parameterValueFactories)
        {
            _parameterValueFactories = parameterValueFactories;
        }

        public bool Qualifies(Type targetType)
        {
            return targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>);
        }

        public object Create(Type targetType, ExecutableParameterInfo parameter)
        {
            if (parameter == null)
                return null;

            var keyType = targetType.GetGenericArguments().First();
            var valueType = targetType.GetGenericArguments().Last();

            var keyParamFactory = _parameterValueFactories.ToList()
                .FirstOrDefault(f => f.Qualifies(keyType));
            if (keyParamFactory == null)
                throw new NoQualifyingParameterValueFactoryException(keyType.Name);

            var valueParamFactory = _parameterValueFactories.ToList()
                .FirstOrDefault(f => f.Qualifies(valueType));
            if (valueParamFactory == null)
                throw new NoQualifyingParameterValueFactoryException(valueType.Name);

            var args = parameter.SingleValue
                .Split(new string[] { "[=" }, StringSplitOptions.RemoveEmptyEntries);
            args[args.Length - 1] = args[args.Length - 1].TrimEnd(']');

            if (args.Length != 2)
                throw new InvalidOperationException($"Unable to parse KeyValuePair args ({ args })");

            return Activator.CreateInstance(typeof(KeyValuePair<,>)
                .MakeGenericType(keyType, valueType), new object[]
                {
                    keyParamFactory.Create(keyType, new ExecutableParameterInfo(string.Empty, args[0].TrimStart('"').TrimEnd('"'))),
                    valueParamFactory.Create(valueType, new ExecutableParameterInfo(string.Empty, args[1]))
                });
        }
    }
}
