using EventSourcingDemo.Domain.Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EventSourcingDemo.Domain.Common.Serialization
{
    public class NameConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Name);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null)
                return;

            var jObject = JObject.FromObject(value);
            var codeValue = jObject.GetValue("Value") as JValue;

            codeValue?.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if(reader.TokenType == JsonToken.String)
            {
                var value = reader.Value;

                if (value != null)
                    return new Name((string) value);
            }

            throw new InvalidOperationException("Unable to deserialize Name.");
        }
    }
}
