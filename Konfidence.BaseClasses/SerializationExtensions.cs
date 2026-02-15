using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Konfidence.Base;

// ReSharper disable UnusedMember.Global
public static class SerializationExtensions
{
    private static readonly JsonSerializerOptions _serializationOptions;
    private static readonly JsonSerializerOptions _deserializationOptions;
    private static readonly JsonSerializerOptions _caseSensitiveDeserializationOptions;
    private static readonly JsonSerializerOptions _cloneOptions;

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

        _cloneOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { stringEnumConverter }
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

    public static bool DeserializeCsv<T>(
        this string toDeserializeDto,
        out List<T> deserializedDto)
    {
        CsvConfiguration config = new(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header.ToLower()
        };

        using CsvReader csvReader = new(new StringReader(toDeserializeDto), config);

        deserializedDto = csvReader.GetRecords<T>().ToList();

        return deserializedDto.Any();
    }

    private static string CloneSerialize<T>(this T toSerializeDto)
    {
        return JsonSerializer.Serialize(toSerializeDto, _cloneOptions);
    }

    private static bool CloneDeserialize<T>(this string toDeserializeDto, [NotNullWhen(true)] out T? deserializedDto)
    {
        deserializedDto = JsonSerializer.Deserialize<T>(toDeserializeDto, _cloneOptions);

        return deserializedDto.IsAssigned();
    }

    // TODO : Consider using a more efficient cloning method if TT is known to be a reference type and supports ICloneable or similar.
    //        use a deep clone library like Force.deepCloner or FastDeepCloner if performance is a concern.
    public static T Clone<T>(this T data)
    {
        return data.CloneSerialize().CloneDeserialize(out T? clonedData)
            ? clonedData
            : data;
    }
}