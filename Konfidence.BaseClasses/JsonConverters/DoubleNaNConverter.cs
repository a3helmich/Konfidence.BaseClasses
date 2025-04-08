using System.Text.Json.Serialization;
using System.Text.Json;
using System;

namespace Konfidence.Base.JsonConverters;

public class DoubleNaNConverter : JsonConverter<double>
{
    public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
            {
                string? value = reader.GetString();
                return value switch
                {
                    "NaN" => double.NaN,
                    "Infinity" => double.PositiveInfinity,
                    "-Infinity" => double.NegativeInfinity,
                    _ => throw new JsonException($"Invalid string value '{value}' for double.")
                };
            }
            case JsonTokenType.Number:
            {
                return reader.GetDouble();
            }
            default:
            {
                throw new JsonException($"Unexpected token {reader.TokenType}.");
            }
        }
    }

    public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
    {
        if (double.IsNaN(value))
        {
            writer.WriteStringValue("NaN");

            return;
        }

        if (double.IsPositiveInfinity(value))
        {
            writer.WriteStringValue("Infinity");

            return;
        }

        if (double.IsNegativeInfinity(value))
        {
            writer.WriteStringValue("-Infinity");

            return;
        }

        writer.WriteNumberValue(value);
    }
}
