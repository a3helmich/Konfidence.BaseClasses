using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using FluentAssertions;
using Konfidence.Base;
using Konfidence.SqlHostProvider.SqlAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog.Events;

namespace Konfidence.BaseClasses.UnitTest;

[TestClass]
public class SerializationExtensionsTests
{
    [TestMethod]
    public void Deserialize_SerializedClientSettings_RoundTripsSuccessfully()
    {
        // Arrange
        ClientSettings clientSettings = new()
        {
            DataConfiguration = new DataConfiguration { Connections = [new ConfigConnectionString()] }
        };

        string clientSettingsSerialised = clientSettings.Serialize();

        // Act
        bool serializationResult = clientSettingsSerialised.Deserialize(out ClientSettings? clientSettingsDeserialized);

        // Assert
        serializationResult.Should().BeTrue();
        clientSettingsDeserialized.Should().NotBeNull();

        if (!clientSettingsDeserialized.IsAssigned())
        {
            return;
        }

        clientSettingsDeserialized.LogLevel.Should().Be(LogEventLevel.Information);
        clientSettingsDeserialized.DataConfiguration.Should().NotBeNull();
        clientSettingsDeserialized.DataConfiguration!.Connections.Should().HaveCount(1);
    }

    private class TestClass
    {
        public double Value { get; set; }
    }

    [TestMethod]
    public void Deserialize_NaNValue_ReturnsConfiguredDouble()
    {
        // Arrange
        string json = "{ \"Value\": \"NaN\" }";

        // Act
        bool serializationResult = json.Deserialize(out TestClass? testClass);

        // Assert
        serializationResult.Should().BeTrue();
        testClass.Should().NotBeNull();

        if (!testClass.IsAssigned())
        {
            return;
        }

        testClass.Value.Should().Be(double.NaN);
    }

    private class SimpleDto
    {
        public string Name { get; set; } = string.Empty;

        public int Count { get; set; }
    }

    private class DtoWithNullableProperty
    {
        public string? OptionalValue { get; set; }
    }

    private class DtoWithIgnoredProperty
    {
        public string Visible { get; set; } = string.Empty;

        [JsonIgnore]
        public string Hidden { get; set; } = string.Empty;
    }

    private class DtoWithList
    {
        public List<string> Items { get; set; } = [];
    }

    private class CsvRow
    {
        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }
    }

    [TestMethod]
    public void SerializeBytes_ThenDeserializeBytes_RoundTripsSuccessfully()
    {
        // Arrange
        SimpleDto dto = new() { Name = "test", Count = 3 };

        // Act
        byte[] bytes = dto.SerializeBytes();
        bool result = ((ReadOnlySpan<byte>)bytes).Deserialize(out SimpleDto? deserialized);

        // Assert
        result.Should().BeTrue();
        deserialized.Should().NotBeNull();
        deserialized!.Name.Should().Be("test");
        deserialized.Count.Should().Be(3);
    }

    [TestMethod]
    public void SerializeBytes_WithCompression_ProducesShorterOutputThanUncompressed()
    {
        // Arrange
        SimpleDto dto = new() { Name = "test", Count = 3 };

        // Act
        byte[] uncompressed = dto.SerializeBytes();
        byte[] compressed = dto.SerializeBytes(withCompression: true);

        // Assert
        // withCompression selects the WriteIndented: false options - without the indentation
        // whitespace, the compressed form must always be shorter than the indented one.
        compressed.Length.Should().BeLessThan(uncompressed.Length);
    }

    [TestMethod]
    public void Serialize_ObjectWithNullProperty_OmitsNullProperty()
    {
        // Arrange
        DtoWithNullableProperty dto = new() { OptionalValue = null };

        // Act
        string json = dto.Serialize();

        // Assert
        // DefaultIgnoreCondition: JsonIgnoreCondition.WhenWritingNull means a null property is
        // dropped from the output entirely, not written as "OptionalValue": null.
        json.Should().NotContain("OptionalValue");
    }

    [TestMethod]
    public void Serialize_Default_ProducesIndentedMultilineJson()
    {
        // Arrange
        SimpleDto dto = new() { Name = "test", Count = 3 };

        // Act
        string json = dto.Serialize();

        // Assert
        json.Should().Contain("\n");
    }

    [TestMethod]
    public void Serialize_WithCompression_ProducesSingleLineJson()
    {
        // Arrange
        SimpleDto dto = new() { Name = "test", Count = 3 };

        // Act
        string json = dto.Serialize(withCompression: true);

        // Assert
        json.Should().NotContain("\n");
    }

    [TestMethod]
    public void Clone_ObjectWithMutableList_ProducesIndependentDeepCopy()
    {
        // Arrange
        DtoWithList original = new() { Items = ["a", "b"] };

        // Act
        DtoWithList clone = original.Clone();
        original.Items.Add("c");

        // Assert
        clone.Items.Should().BeEquivalentTo(["a", "b"]);
        original.Items.Should().BeEquivalentTo(["a", "b", "c"]);
    }

    [TestMethod]
    public void Clone_ObjectWithJsonIgnoreProperty_StillLosesIgnoredPropertyDespiteCustomResolver()
    {
        // Arrange
        DtoWithIgnoredProperty dto = new() { Visible = "shown", Hidden = "secret" };

        // Act
        DtoWithIgnoredProperty clone = dto.Clone();

        // Assert
        // _cloneOptions' resolver forces ShouldSerialize = true for [JsonIgnore] properties, so
        // "secret" does get written into the intermediate clone JSON - but CloneDeserialize() reads
        // it back with plain JsonSerializer.Deserialize(), which still honors [JsonIgnore]'s default
        // "Always" condition on the read side and drops the property again. The resolver only
        // patches the write half of the round trip, so Clone() ends up no different from a regular
        // Serialize()/Deserialize() round trip here - a real gap between what the resolver's comment
        // implies and what Clone() actually preserves.
        clone.Visible.Should().Be("shown");
        clone.Hidden.Should().BeEmpty();
    }

    [TestMethod]
    public void Clone_NullValue_ReturnsNull()
    {
        // Arrange
        string? value = null;

        // Act
        string? result = value.Clone<string?>();

        // Assert
        // CloneSerialize("null").CloneDeserialize() yields a null T, so IsAssigned() is false and
        // Clone() falls back to returning the original (also null) value instead of clonedData -
        // this is the only way to reach that fallback branch.
        result.Should().BeNull();
    }

    [TestMethod]
    public void Deserialize_BytesWithInvalidJson_ReturnsFalse()
    {
        // Arrange
        byte[] bytes = Encoding.UTF8.GetBytes("{ invalid json");

        // Act
        bool result = ((ReadOnlySpan<byte>)bytes).Deserialize(out SimpleDto? deserialized);

        // Assert
        result.Should().BeFalse();
        deserialized.Should().BeNull();
    }

    [TestMethod]
    public void Deserialize_BytesCaseSensitiveWithMismatchedCasing_LeavesPropertiesAtDefault()
    {
        // Arrange
        // The case-sensitive options are still built on JsonSerializerDefaults.Web, which sets
        // PropertyNamingPolicy to CamelCase - so "name"/"count" (camelCase) is actually the
        // *correctly* cased key, and PascalCase "Name"/"Count" is what fails to bind once
        // PropertyNameCaseInsensitive is forced to false.
        byte[] bytes = Encoding.UTF8.GetBytes("{ \"Name\": \"test\", \"Count\": 3 }");

        // Act
        bool result = ((ReadOnlySpan<byte>)bytes).Deserialize(out SimpleDto? deserialized, caseSensitive: true);

        // Assert
        // Deserialization still succeeds (a non-null object is produced), but every property is
        // left at its default since neither key matched the expected camelCase form.
        result.Should().BeTrue();
        deserialized.Should().NotBeNull();
        deserialized!.Name.Should().BeEmpty();
        deserialized.Count.Should().Be(0);
    }

    [TestMethod]
    public void Deserialize_StringWithInvalidJson_ReturnsFalse()
    {
        // Arrange
        string json = "{ invalid json";

        // Act
        bool result = json.Deserialize(out SimpleDto? deserialized);

        // Assert
        result.Should().BeFalse();
        deserialized.Should().BeNull();
    }

    [TestMethod]
    public void Deserialize_StringCaseSensitiveWithMismatchedCasing_LeavesPropertiesAtDefault()
    {
        // Arrange
        // See the equivalent bytes-overload test for why PascalCase, not lowercase, is the
        // mismatched casing here - JsonSerializerDefaults.Web expects camelCase keys.
        string json = "{ \"Name\": \"test\", \"Count\": 3 }";

        // Act
        bool result = json.Deserialize(out SimpleDto? deserialized, caseSensitive: true);

        // Assert
        result.Should().BeTrue();
        deserialized.Should().NotBeNull();
        deserialized!.Name.Should().BeEmpty();
        deserialized.Count.Should().Be(0);
    }

    [TestMethod]
    public void DeserializeCsv_ValidCsv_ReturnsRecords()
    {
        // Arrange
        string csv = "Name,Age\r\nAlice,30\r\nBob,25\r\n";

        // Act
        bool result = csv.DeserializeCsv(out List<CsvRow> rows);

        // Assert
        result.Should().BeTrue();
        rows.Should().HaveCount(2);
        rows[0].Name.Should().Be("Alice");
        rows[0].Age.Should().Be(30);
        rows[1].Name.Should().Be("Bob");
        rows[1].Age.Should().Be(25);
    }

    [TestMethod]
    public void DeserializeCsv_HeaderOnlyCsv_ReturnsFalse()
    {
        // Arrange
        string csv = "Name,Age\r\n";

        // Act
        bool result = csv.DeserializeCsv(out List<CsvRow> rows);

        // Assert
        result.Should().BeFalse();
        rows.Should().BeEmpty();
    }

    [TestMethod]
    public void DeserializeCsv_MalformedCsv_ReturnsFalse()
    {
        // Arrange
        // An unterminated quoted field is invalid CSV that CsvHelper's default (strict) mode
        // throws on while reading, not just something it parses into an empty result - this is
        // the only way to reach the catch block, distinct from the header-only "no rows" case.
        string csv = "Name,Age\r\n\"Alice,30\r\n";

        // Act
        bool result = csv.DeserializeCsv(out List<CsvRow> rows);

        // Assert
        result.Should().BeFalse();
        rows.Should().BeEmpty();
    }
}