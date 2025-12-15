using System;
using Newtonsoft.Json;

namespace VVRestApi.Common.Serialize
{
    public class NullableGuidConverter : JsonConverter<Guid?>
    {
        public override Guid? ReadJson(
            JsonReader reader,
            Type objectType,
            Guid? existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            if (reader.TokenType == JsonToken.String)
            {
                var value = reader.Value?.ToString();
                if (string.IsNullOrEmpty(value))
                    return null;

                if (Guid.TryParse(value, out var guid))
                    return guid;
            }

            throw new JsonSerializationException(
                $"Expected string or null for Guid?, got {reader.TokenType}");
        }

        public override void WriteJson(
            JsonWriter writer,
            Guid? value,
            JsonSerializer serializer)
        {
            if (value.HasValue)
                writer.WriteValue(value.Value.ToString());
            else
                writer.WriteNull();
        }
    }
}
