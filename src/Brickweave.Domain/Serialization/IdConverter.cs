using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Brickweave.Domain.Serialization
{
    public class IdConverter : JsonConverter
    {
        private static readonly IEnumerable<Type> SupportedTypes = new List<Type>
        {
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(ulong),
            typeof(decimal),
            typeof(float),
            typeof(double),
            typeof(char),
            typeof(Guid),
            typeof(string)
        };

        public override bool CanConvert(Type objectType)
        {
            return SupportedTypes
                .Any(t => typeof(Id<>).MakeGenericType(t).IsAssignableFrom(objectType.GetTypeInfo()));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var jObject = JObject.FromObject(value);
            var idValue = jObject.GetValue("Value") as JValue;

            idValue?.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var constructor = objectType.GetConstructors()
                .Where(c => c.GetParameters().Length == 1)
                .FirstOrDefault();

            if (constructor == null)
                throw new InvalidOperationException($"Unable to deserialize {objectType}. Cannot find suitable constructor.");

            if (reader.Value == null)
                return null;

            var currentType = reader.Value.GetType();
            var targetType = constructor.GetParameters().First().ParameterType;

            if(currentType == targetType)
                return constructor.Invoke(new[] { reader.Value });

            var isGuid = Guid.TryParse(reader.Value.ToString(), out Guid guid);
            if(isGuid)
                return constructor.Invoke(new object[] { guid });

            return Convert.ChangeType(reader.Value, targetType);
        }
    }
}
