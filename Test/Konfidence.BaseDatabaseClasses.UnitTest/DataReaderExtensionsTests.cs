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
    public void GetField_decimal_With_value_Should_return_value()
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
    public void GetField_decimal_With_DBNull_Should_return_zero()
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
    public void GetField_XmlDocument_Should_load_xml_from_string_field()
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
    public void GetField_TimeSpan_With_value_Should_return_value()
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
    public void GetField_TimeSpan_With_DBNull_Should_return_MinValue()
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
}
