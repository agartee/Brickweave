using System;
using System.Collections.Generic;
using System.Linq;
using Brickweave.Serialization.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.JsonNet;

namespace Brickweave.Serialization
{
    public class JsonDocumentSerializer : IDocumentSerializer
    {
        private readonly IEnumerable<Type> _shorthandTypes;
        private readonly IEnumerable<JsonConverter> _converters;

        public JsonDocumentSerializer(params Type[] shorthandTypes)
        {
            _shorthandTypes = shorthandTypes;
            _converters = Enumerable.Empty<JsonConverter>();
        }

        public JsonDocumentSerializer(IEnumerable<Type> shorthandTypes, params JsonConverter[] converters)
        {
            _shorthandTypes = shorthandTypes;
            _converters = converters;
        }

        private JsonSerializerSettings DefaultSettings =>
            new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            }
            .ConfigureForNodaTime(DateTimeZoneProviders.Tzdb)
            .AddJsonConverters(_converters.ToArray());

        public string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj, DefaultSettings);
        }

        public T DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, DefaultSettings);
        }

        public T DeserializeObject<T>(string typeName, string json)
        {
            var type = _shorthandTypes.FirstOrDefault(t => t.Name == typeName)
                ?? Type.GetType(typeName);

            return (T)JsonConvert.DeserializeObject(json, type, DefaultSettings);
        }

        public object DeserializeObject(string typeName, string json)
        {
            var type = _shorthandTypes.FirstOrDefault(t => t.Name == typeName)
                ?? Type.GetType(typeName);
            
            return JsonConvert.DeserializeObject(json, type, DefaultSettings);
        }
    }
}
