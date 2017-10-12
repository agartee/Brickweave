using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Serialization;

namespace Brickweave.EventStore.Serialization
{
    public class ShortNameBinder : ISerializationBinder
    {
        private readonly IEnumerable<Type> _shorthandTypes;

        public ShortNameBinder(IEnumerable<Type> shorthandTypes)
        {
            _shorthandTypes = shorthandTypes;
        }

        public Type BindToType(string assemblyName, string typeName)
        {
            return string.IsNullOrWhiteSpace(assemblyName)
                ? _shorthandTypes.First(t => t.Name == typeName)
                : Type.GetType($"{typeName},{assemblyName}");
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            if (_shorthandTypes.Contains(serializedType))
            {
                assemblyName = null;
                typeName = serializedType.Name;
            }
            else
            {
                assemblyName = serializedType.Assembly.FullName;
                typeName = serializedType.FullName;
            }
        }
    }
}
