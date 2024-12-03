using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Konfidence.Base
{
    public static class SerializationExtensions
    {
        private static readonly JsonSerializerOptions _serializationOptions;
        private static readonly JsonSerializerOptions _deserializationOptions;
        private static readonly JsonSerializerOptions _caseSensitiveDeserializationOptions;

        static SerializationExtensions()
        {
            JsonStringEnumConverter stringEnumConverter = new();

            _serializationOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters = { stringEnumConverter }
            };

            _deserializationOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                Converters = { stringEnumConverter }
            };

            _caseSensitiveDeserializationOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                Converters = { stringEnumConverter },
                PropertyNameCaseInsensitive = false
            };
        }

        public static string Serialize<T>(this T toSerializeDto)
        {
            return JsonSerializer.Serialize(toSerializeDto, _serializationOptions);
        }

        public static bool Deserialize<T>(this string toDeserializeDto, [NotNullWhen(true)] out T? deserializedDto, bool caseSensitive = false)
        {
            if (caseSensitive)
            {
                deserializedDto = JsonSerializer.Deserialize<T>(toDeserializeDto, _caseSensitiveDeserializationOptions);

                return deserializedDto.IsAssigned();
            }

            deserializedDto = JsonSerializer.Deserialize<T>(toDeserializeDto, _deserializationOptions);

            return deserializedDto.IsAssigned();
        }
    }
}