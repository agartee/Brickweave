using System;
using System.Collections.Generic;
using System.Linq;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Factories.ParameterValues
{
    public class DictionaryParameterValueFactory : IParameterValueFactory
    {
        private readonly KeyValuePairParameterValueFactory _keyValuePairParameterValueFactory;
        
        public DictionaryParameterValueFactory(KeyValuePairParameterValueFactory keyValuePairParameterValueFactory)
        {
            _keyValuePairParameterValueFactory = keyValuePairParameterValueFactory;
        }

        public bool Qualifies(Type targetType)
        {
            return targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(IDictionary<,>)
                || targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        }

        public object Create(Type targetType, ExecutableParameterInfo parameter)
        {
            if (parameter == null)
                return null;

            var keyType = targetType.GetGenericArguments().First();
            var valueType = targetType.GetGenericArguments().Last();

            var dictionary = (dynamic) Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(keyType, valueType));

            if (IsEnumerable(valueType) || IsList(valueType))
            {
                var valueGenericType = valueType.GetGenericArguments().First();

                var keyValuePairGroups = parameter.Values
                    .Select(v => (dynamic)_keyValuePairParameterValueFactory
                        .Create(typeof(KeyValuePair<,>).MakeGenericType(keyType, valueGenericType),
                        new ExecutableParameterInfo(string.Empty, v)))
                    .GroupBy(kvp => kvp.Key)
                    .ToList();

                foreach(var grp in keyValuePairGroups)
                {
                    var list = (dynamic)Activator.CreateInstance(typeof(List<>).MakeGenericType(valueGenericType));

                    foreach (var value in grp.Select(kvp => kvp.Value))
                        list.Add(value);

                    dictionary.Add(grp.Key, IsList(valueType) ? list : list.ToArray());
                }

                return dictionary;
            }
            else
            {
                var keyValuePairs = parameter.Values
                    .Select(v => (dynamic)_keyValuePairParameterValueFactory
                        .Create(typeof(KeyValuePair<,>).MakeGenericType(keyType, valueType),
                        new ExecutableParameterInfo(string.Empty, v)))
                    .ToList();

                foreach (var kvp in keyValuePairs)
                    dictionary.Add(kvp.Key, kvp.Value);

                return dictionary;
            }
        }

        private static bool IsEnumerable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }

        private static bool IsList(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)
                || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>);
        }
    }
}
