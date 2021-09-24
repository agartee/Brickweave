using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Brickweave.Domain;

namespace BasicCqrs.WebApp.Converters
{
    /// <summary>
    /// This converter facilitates the flattening of "Id" value objects before sending them down to the CLI
    /// client. The API endpoint in this demo application used by the CLI does not include any DTO conversion 
    /// of domain models.
    /// </summary>
    /// <typeparam name="T">The Brickweave.Domain.Id implementation class</typeparam>
    public class FlatIdConverter<T> : JsonConverter<T> where T : Id<Guid>
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotSupportedException();
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
