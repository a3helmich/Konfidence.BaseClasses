using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Konfidence.Base
{
    public static class SerializationExtensions
    {
        public static string Serialize<T>(this T toSerializeDto)
        {
            return System.Text.Json.JsonSerializer.Serialize(toSerializeDto, new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
        }

        public static bool Deserialize<T>(this string toDeserializeDto, [NotNullWhen(true)] out T? deserializedDto)
        {
            deserializedDto = System.Text.Json.JsonSerializer.Deserialize<T>(toDeserializeDto, new JsonSerializerOptions { AllowTrailingCommas = true });

            return deserializedDto.IsAssigned();
        }
    }
}