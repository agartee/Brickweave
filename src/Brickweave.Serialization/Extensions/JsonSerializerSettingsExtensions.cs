using Newtonsoft.Json;

namespace Brickweave.Serialization.Extensions
{
    public static class JsonSerializerSettingsExtensions
    {
        public static JsonSerializerSettings AddJsonConverters(this JsonSerializerSettings settings, params JsonConverter[] converters)
        {
            foreach(var converter in converters)
                settings.Converters.Add(converter);

            return settings;
        }
    }
}
