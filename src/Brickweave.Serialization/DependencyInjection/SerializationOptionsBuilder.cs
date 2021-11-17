using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Brickweave.Serialization.DependencyInjection
{
    public class SerializationOptionsBuilder
    {
        private readonly IList<JsonConverter> _converters = new List<JsonConverter>();

        public SerializationOptionsBuilder(IServiceCollection services, params Type[] shorthandTypes)
        {
            services.AddSingleton<IDocumentSerializer>(s => 
                new JsonDocumentSerializer(shorthandTypes, _converters.ToArray()));
        }

        public SerializationOptionsBuilder AddJsonConverter(JsonConverter converter)
        {
            _converters.Add(converter);
            return this;
        }
    }
}
