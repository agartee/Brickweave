using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Brickweave.Samples.SqlServer.Serialization
{
    public static class SnapshotSerializer
    {
        private static JsonSerializerSettings DefaultSettings =>
            new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                TypeNameHandling = TypeNameHandling.Objects,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

        public static T DeserializeObject<T>(string json)
        {
            return (T)JsonConvert.DeserializeObject(json, DefaultSettings);
        }

        public static string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj, DefaultSettings);
        }
    }
}
