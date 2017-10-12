using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Brickweave.Samples.WebApp.Tests.Extensions
{
    public static class SerializationExtensions
    {
        private static JsonSerializerSettings Settings => new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static StringContent ToStringContent(this object obj)
        {
            return new StringContent(
                JsonConvert.SerializeObject(obj, Settings),
                Encoding.UTF8,
                "application/json");
        }

        public static JObject ToJObject(this string json)
        {
            return JsonConvert.DeserializeObject<JObject>(json, Settings);
        }

        public static JObject[] ToJObjects(this string json)
        {
            return JsonConvert.DeserializeObject<List<JObject>>(json, Settings).ToArray();
        }
    }
}
