using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.JsonNet;

namespace Brickweave.EventStore.Serialization
{
    public class JsonDocumentSerializer : IDocumentSerializer
    {
        private readonly IEnumerable<Type> _shorthandTypes;
        
        public JsonDocumentSerializer(params Type[] shorthandTypes)
        {
            _shorthandTypes = shorthandTypes;
        }

        private JsonSerializerSettings DefaultSettings =>
            new JsonSerializerSettings
            {
                SerializationBinder = new ShortNameBinder(_shorthandTypes),
                Formatting = Formatting.None,
                TypeNameHandling = TypeNameHandling.Objects,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

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
