using System;
using System.Data;
using System.Xml;
using FluentAssertions;
using Konfidence.BaseData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Konfidence.BaseDatabaseClasses.UnitTest;

[TestClass]
public class DataReaderExtensionsTests
{
    [TestMethod]
    public void GetField_DecimalWithValue_ReturnsValue()
    {
        // Arrange
        Mock<IDataReader> dataReaderMock = new();
        dataReaderMock.Setup(x => x.GetOrdinal("Field")).Returns(0);
        dataReaderMock.Setup(x => x.IsDBNull(0)).Returns(false);
        dataReaderMock.Setup(x => x.GetDecimal(0)).Returns(7.5m);

        // Act
        dataReaderMock.Object.GetField("Field", out decimal field);

        // Assert
        field.Should().Be(7.5m);
    }

    [TestMethod]
    public void GetField_DecimalWithDBNull_ReturnsZero()
    {
        // Arrange
        Mock<IDataReader> dataReaderMock = new();
        dataReaderMock.Setup(x => x.GetOrdinal("Field")).Returns(0);
        dataReaderMock.Setup(x => x.IsDBNull(0)).Returns(true);

        // Act
        dataReaderMock.Object.GetField("Field", out decimal field);

        // Assert
        field.Should().Be(0);
    }

    [TestMethod]
    public void GetField_XmlDocument_LoadsXmlFromStringField()
    {
        // Arrange
        Mock<IDataReader> dataReaderMock = new();
        dataReaderMock.Setup(x => x.GetOrdinal("Field")).Returns(0);
        dataReaderMock.Setup(x => x.IsDBNull(0)).Returns(false);
        dataReaderMock.Setup(x => x.GetString(0)).Returns("<root><child>value</child></root>");
        XmlDocument field = new();

        // Act
        dataReaderMock.Object.GetField("Field", ref field);

        // Assert
        field.DocumentElement?.Name.Should().Be("root");
        field.DocumentElement?.FirstChild?.InnerText.Should().Be("value");
    }

    [TestMethod]
    public void GetField_TimeSpanWithValue_ReturnsValue()
    {
        // Arrange
        Mock<IDataReader> dataReaderMock = new();
        TimeSpan storedValue = TimeSpan.FromHours(2);
        dataReaderMock.Setup(x => x.GetOrdinal("Field")).Returns(0);
        dataReaderMock.Setup(x => x.IsDBNull(0)).Returns(false);
        dataReaderMock.Setup(x => x.GetValue(0)).Returns(storedValue);

        // Act
        dataReaderMock.Object.GetField("Field", out TimeSpan field);

        // Assert
        field.Should().Be(storedValue);
    }

    [TestMethod]
    public void GetField_TimeSpanWithDBNull_ReturnsMinValue()
    {
        // Arrange
        Mock<IDataReader> dataReaderMock = new();
        dataReaderMock.Setup(x => x.GetOrdinal("Field")).Returns(0);
        dataReaderMock.Setup(x => x.IsDBNull(0)).Returns(true);

        // Act
        dataReaderMock.Object.GetField("Field", out TimeSpan field);

        // Assert
        field.Should().Be(TimeSpan.MinValue);
    }

    // Only the decimal, TimeSpan and XmlDocument overloads had tests - the remaining eight each
    // carry their own DBNull-versus-value branch with its own fallback constant, so both sides of
    // every one of them is covered below.

    [TestMethod]
    public void GetField_ByteWithValue_ReturnsValue()
    {
        // Arrange
        Mock<IDataReader> dataReaderMock = CreateDataReaderMock(isDbNull: false);
        dataReaderMock.Setup(x => x.GetByte(0)).Returns(7);

        // Act
        dataReaderMock.Object.GetField("Field", out byte field);

        // Assert
        field.Should().Be(7);
    }

    [TestMethod]
    public void GetField_ByteWithDBNull_ReturnsZero()
    {
        // Arrange
        Mock<IDataReader> dataReaderMock = CreateDataReaderMock(isDbNull: true);

        // Act
        dataReaderMock.Object.GetField("Field", out byte field);

        // Assert
        field.Should().Be(0);
    }

    [TestMethod]
    public void GetField_ShortWithValue_ReturnsValue()
    {
        // Arrange
        Mock<IDataReader> dataReaderMock = CreateDataReaderMock(isDbNull: false);
        dataReaderMock.Setup(x => x.GetInt16(0)).Returns(7);

        // Act
        dataReaderMock.Object.GetField("Field", out short field);

        // Assert
        field.Should().Be(7);
    }

    [TestMethod]
    public void GetField_ShortWithDBNull_ReturnsZero()
    {
        // Arrange
        Mock<IDataReader> dataReaderMock = CreateDataReaderMock(isDbNull: true);

        // Act
        dataReaderMock.Object.GetField("Field", out short field);

        // Assert
        field.Should().Be(0);
    }

    [TestMethod]
    public void GetField_IntWithValue_ReturnsValue()
    {
        // Arrange
        Mock<IDataReader> dataReaderMock = CreateDataReaderMock(isDbNull: false);
        dataReaderMock.Setup(x => x.GetInt32(0)).Returns(7);

        // Act
        dataReaderMock.Object.GetField("Field", out int field);

        // Assert
        field.Should().Be(7);
    }

    [TestMethod]
    public void GetField_IntWithDBNull_ReturnsZero()
    {
        // Arrange
        Mock<IDataReader> dataReaderMock = CreateDataReaderMock(isDbNull: true);

        // Act
        dataReaderMock.Object.GetField("Field", out int field);

        // Assert
        field.Should().Be(0);
    }

    [TestMethod]
    public void GetField_LongWithValue_ReturnsValue()
    {
        // Arrange
        Mock<IDataReader> dataReaderMock = CreateDataReaderMock(isDbNull: false);
        dataReaderMock.Setup(x => x.GetInt64(0)).Returns(7L);

        // Act
        dataReaderMock.Object.GetField("Field", out long field);

        // Assert
        field.Should().Be(7L);
    }

    [TestMethod]
    public void GetField_LongWithDBNull_ReturnsZero()
    {
        // Arrange
        Mock<IDataReader> dataReaderMock = CreateDataReaderMock(isDbNull: true);

        // Act
        dataReaderMock.Object.GetField("Field", out long field);

        // Assert
        field.Should().Be(0L);
    }

    [TestMethod]
    public void GetField_BoolWithTrueValue_ReturnsTrue()
    {
        // Arrange
        Mock<IDataReader> dataReaderMock = CreateDataReaderMock(isDbNull: false);
        dataReaderMock.Setup(x => x.GetBoolean(0)).Returns(true);

        // Act
        dataReaderMock.Object.GetField("Field", out bool field);

        // Assert
        field.Should().BeTrue();
    }

    [TestMethod]
    public void GetField_BoolWithDBNull_ReturnsFalse()
    {
        // Arrange
        // The bool overload is the odd one out - it short-circuits with "!IsDBNull && GetBoolean"
        // instead of a ternary, so a null column can never be distinguished from a stored false.
        Mock<IDataReader> dataReaderMock = CreateDataReaderMock(isDbNull: true);

        // Act
        dataReaderMock.Object.GetField("Field", out bool field);

        // Assert
        field.Should().BeFalse();
        dataReaderMock.Verify(x => x.GetBoolean(It.IsAny<int>()), Times.Never);
    }

    [TestMethod]
    public void GetField_GuidWithValue_ReturnsValue()
    {
        // Arrange
        Mock<IDataReader> dataReaderMock = CreateDataReaderMock(isDbNull: false);
        Guid storedValue = Guid.NewGuid();
        dataReaderMock.Setup(x => x.GetGuid(0)).Returns(storedValue);

        // Act
        dataReaderMock.Object.GetField("Field", out Guid field);

        // Assert
        field.Should().Be(storedValue);
    }

    [TestMethod]
    public void GetField_GuidWithDBNull_ReturnsEmpty()
    {
        // Arrange
        Mock<IDataReader> dataReaderMock = CreateDataReaderMock(isDbNull: true);

        // Act
        dataReaderMock.Object.GetField("Field", out Guid field);

        // Assert
        field.Should().Be(Guid.Empty);
    }

    [TestMethod]
    public void GetField_StringWithValue_ReturnsValue()
    {
        // Arrange
        Mock<IDataReader> dataReaderMock = CreateDataReaderMock(isDbNull: false);
        dataReaderMock.Setup(x => x.GetString(0)).Returns("value");

        // Act
        dataReaderMock.Object.GetField("Field", out string field);

        // Assert
        field.Should().Be("value");
    }

    [TestMethod]
    public void GetField_StringWithDBNull_ReturnsEmptyString()
    {
        // Arrange
        Mock<IDataReader> dataReaderMock = CreateDataReaderMock(isDbNull: true);

        // Act
        dataReaderMock.Object.GetField("Field", out string field);

        // Assert
        field.Should().BeEmpty();
    }

    [TestMethod]
    public void GetField_DateTimeWithValue_ReturnsValue()
    {
        // Arrange
        Mock<IDataReader> dataReaderMock = CreateDataReaderMock(isDbNull: false);
        DateTime storedValue = new(2026, 8, 8, 13, 45, 0, DateTimeKind.Utc);
        dataReaderMock.Setup(x => x.GetDateTime(0)).Returns(storedValue);

        // Act
        dataReaderMock.Object.GetField("Field", out DateTime field);

        // Assert
        field.Should().Be(storedValue);
    }

    [TestMethod]
    public void GetField_DateTimeWithDBNull_ReturnsMinValue()
    {
        // Arrange
        Mock<IDataReader> dataReaderMock = CreateDataReaderMock(isDbNull: true);

        // Act
        dataReaderMock.Object.GetField("Field", out DateTime field);

        // Assert
        field.Should().Be(DateTime.MinValue);
    }

    [TestMethod]
    public void GetField_XmlDocumentWithDBNull_ThrowsOnEmptyXml()
    {
        // Arrange
        // The XmlDocument overload delegates to the string overload, which turns a null column
        // into string.Empty - and XmlDocument.LoadXml rejects an empty document rather than
        // leaving the caller's instance untouched.
        Mock<IDataReader> dataReaderMock = CreateDataReaderMock(isDbNull: true);
        XmlDocument field = new();

        // Act
        Action action = () => dataReaderMock.Object.GetField("Field", ref field);

        // Assert
        action.Should().Throw<XmlException>();
    }

    private static Mock<IDataReader> CreateDataReaderMock(bool isDbNull)
    {
        Mock<IDataReader> dataReaderMock = new();

        dataReaderMock.Setup(x => x.GetOrdinal("Field")).Returns(0);
        dataReaderMock.Setup(x => x.IsDBNull(0)).Returns(isDbNull);

        return dataReaderMock;
    }
}
