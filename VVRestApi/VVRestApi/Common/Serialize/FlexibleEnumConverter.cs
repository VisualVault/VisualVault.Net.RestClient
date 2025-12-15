using System;
using Newtonsoft.Json;

namespace VVRestApi.Common.Serialize
{
    public class FlexibleEnumConverter<T> : JsonConverter where T : struct, Enum
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T);
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                throw new JsonSerializationException($"Cannot convert null to {typeof(T).Name}");

            if (reader.TokenType == JsonToken.Integer)
            {
                return Enum.ToObject(typeof(T), Convert.ToInt32(reader.Value));
            }

            if (reader.TokenType == JsonToken.String)
            {
                var stringValue = reader.Value.ToString();

                // Numeric string
                if (int.TryParse(stringValue, out var numeric))
                {
                    return Enum.ToObject(typeof(T), numeric);
                }

                // Enum name
                if (Enum.TryParse<T>(stringValue, true, out var parsed))
                {
                    return parsed;
                }
            }

            throw new JsonSerializationException(
                $"Unable to convert value '{reader.Value}' to enum {typeof(T).Name}");
        }

        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            writer.WriteValue(Convert.ToInt32(value));
        }
    }
}
