using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Konfidence.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseClasses.UnitTest;

[TestClass]
public class SerializationExtensionsMoreTests
{
    private sealed class Dto
    {
        public string Name { get; set; } = string.Empty;

        public int Value { get; set; }
    }

    [TestMethod]
    public void Serialize_WithoutCompression_ReturnsIndentedJson()
    {
        // Arrange
        Dto dto = new() { Name = "Test", Value = 7 };

        // Act
        string json = dto.Serialize();

        // Assert
        json.Should().Contain("\n");
        json.Should().Contain("Test");
    }

    [TestMethod]
    public void Serialize_WithCompression_ReturnsSingleLineJson()
    {
        // Arrange
        Dto dto = new() { Name = "Test", Value = 7 };

        // Act
        string json = dto.Serialize(withCompression: true);

        // Assert
        json.Should().NotContain("\n");
        json.Should().Contain("Test");
    }

    [TestMethod]
    public void SerializeBytes_AnyDto_ReturnsUtf8EncodedJson()
    {
        // Arrange
        Dto dto = new() { Name = "Test", Value = 7 };

        // Act
        byte[] bytes = dto.SerializeBytes();

        // Assert
        Encoding.UTF8.GetString(bytes).Should().Contain("Test");
    }

    [TestMethod]
    public void Clone_AnyDto_ReturnsEquivalentButDistinctInstance()
    {
        // Arrange
        Dto dto = new() { Name = "Test", Value = 7 };

        // Act
        Dto clone = dto.Clone();

        // Assert
        clone.Should().NotBeSameAs(dto);
        clone.Name.Should().Be(dto.Name);
        clone.Value.Should().Be(dto.Value);
    }

    [TestMethod]
    public void Deserialize_ReadOnlySpanOfBytes_ReturnsDeserializedDto()
    {
        // Arrange
        byte[] bytes = Encoding.UTF8.GetBytes("{\"Name\":\"Test\",\"Value\":7}");

        // Act
        bool result = ((ReadOnlySpan<byte>)bytes).Deserialize(out Dto? dto);

        // Assert
        result.Should().BeTrue();
        dto.Should().NotBeNull();
        dto!.Name.Should().Be("Test");
        dto.Value.Should().Be(7);
    }

    [TestMethod]
    public void Deserialize_ReadOnlySpanOfInvalidBytes_ReturnsFalse()
    {
        // Arrange
        byte[] bytes = Encoding.UTF8.GetBytes("not json");

        // Act
        bool result = ((ReadOnlySpan<byte>)bytes).Deserialize(out Dto? dto);

        // Assert
        result.Should().BeFalse();
        dto.Should().BeNull();
    }

    [TestMethod]
    public void Deserialize_CaseSensitiveWithCamelCaseJson_PopulatesProperties()
    {
        // Arrange
        // The case-sensitive options are still based on JsonSerializerDefaults.Web, so property
        // names are expected in camelCase even though matching against them is case-sensitive.
        string json = "{\"name\":\"Test\",\"value\":7}";

        // Act
        bool result = json.Deserialize(out Dto? dto, caseSensitive: true);

        // Assert
        result.Should().BeTrue();
        dto.Should().NotBeNull();
        dto!.Name.Should().Be("Test");
        dto.Value.Should().Be(7);
    }

    [TestMethod]
    public void Deserialize_CaseSensitiveWithPascalCaseJson_LeavesPropertiesAtDefault()
    {
        // Arrange
        string json = "{\"Name\":\"Test\",\"Value\":7}";

        // Act
        bool result = json.Deserialize(out Dto? dto, caseSensitive: true);

        // Assert
        result.Should().BeTrue();
        dto.Should().NotBeNull();
        dto!.Name.Should().BeEmpty();
        dto.Value.Should().Be(0);
    }

    [TestMethod]
    public void Deserialize_InvalidJson_ReturnsFalse()
    {
        // Arrange
        string json = "not json";

        // Act
        bool result = json.Deserialize(out Dto? dto);

        // Assert
        result.Should().BeFalse();
        dto.Should().BeNull();
    }

    [TestMethod]
    public void DeserializeCsv_ValidCsv_ReturnsParsedRecords()
    {
        // Arrange
        string csv = "Name,Value\nFirst,1\nSecond,2";

        // Act
        bool result = csv.DeserializeCsv(out List<Dto> records);

        // Assert
        result.Should().BeTrue();
        records.Should().HaveCount(2);
        records[0].Name.Should().Be("First");
        records[0].Value.Should().Be(1);
        records[1].Name.Should().Be("Second");
        records[1].Value.Should().Be(2);
    }

    [TestMethod]
    public void DeserializeCsv_EmptyCsv_ReturnsFalse()
    {
        // Arrange
        string csv = string.Empty;

        // Act
        bool result = csv.DeserializeCsv(out List<Dto> records);

        // Assert
        result.Should().BeFalse();
        records.Should().BeEmpty();
    }
}
