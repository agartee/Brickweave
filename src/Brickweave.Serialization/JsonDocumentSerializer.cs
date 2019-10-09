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
                SerializationBinder = new ShortNameBinder(_shorthandTypes),
                Formatting = Formatting.None,
                TypeNameHandling = TypeNameHandling.Objects,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy
                    {
                        ProcessDictionaryKeys = true
                    }
                }
            }
            .ConfigureForNodaTime(DateTimeZoneProviders.Tzdb)
            .AddJsonConverters(_converters.ToArray());

        T IDocumentSerializer.DeserializeObject<T>(string json)
        {
            return (T) JsonConvert.DeserializeObject(json, DefaultSettings);
        }

        public string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj, DefaultSettings);
        }
    }
}
