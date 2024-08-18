using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Konfidence.Base
{
    public static class SerializationExtensions
    {
        public static string Serialize<T>(this T toSerializeDto)
        {
            JsonStringEnumConverter stringEnumConverter = new();

            JsonSerializerOptions serializationOptions = new() { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, Converters = { stringEnumConverter } };

            return JsonSerializer.Serialize(toSerializeDto, serializationOptions);
        }

        public static bool Deserialize<T>(this string toDeserializeDto, [NotNullWhen(true)] out T? deserializedDto)
        {
            JsonStringEnumConverter stringEnumConverter = new();

            JsonSerializerOptions serializationOptions = new() { AllowTrailingCommas = true, Converters = { stringEnumConverter } };

            deserializedDto = JsonSerializer.Deserialize<T>(toDeserializeDto, serializationOptions);

            return deserializedDto.IsAssigned();
        }
    }
}