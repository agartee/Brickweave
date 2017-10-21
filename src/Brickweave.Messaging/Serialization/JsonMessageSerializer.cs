using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.JsonNet;

namespace Brickweave.Messaging.Serialization
{
    public class JsonMessageSerializer : IMessageSerializer
    {
        public T DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, DefaultSettings);
        }

        public string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj, DefaultSettings);
        }

        public static JsonSerializerSettings DefaultSettings =>
            new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
    }
}
