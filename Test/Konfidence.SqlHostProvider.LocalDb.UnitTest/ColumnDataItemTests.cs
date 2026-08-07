using FluentAssertions;
using Konfidence.SqlHostProvider.SqlDbSchema;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.SqlHostProvider.LocalDb.UnitTest;

[TestClass]
public class ColumnDataItemTests
{
    [TestMethod]
    public void GetDbDataType_DataTypeWithRepeatedLeadingCharacter_CapitalizesOnlyFirstCharacter()
    {
        // Arrange
        ColumnDataItem columnDataItem = new();

        // Act
        string result = columnDataItem.GetDbDataType("ssn");

        // Assert
        // Before the fix, `dataType.TrimStart(dataType[0])` stripped every leading character equal
        // to the first one, not just the first character itself, so "ssn" became "Sn" instead of "Ssn".
        result.Should().Be("Ssn");
    }

    [TestMethod]
    public void GetDbDataType_Int_ReturnsInt32()
    {
        // Arrange
        ColumnDataItem columnDataItem = new();

        // Act
        string result = columnDataItem.GetDbDataType("int");

        // Assert
        result.Should().Be("Int32");
    }

    [TestMethod]
    public void GetDbDataType_Short_ReturnsShort16()
    {
        // Arrange
        ColumnDataItem columnDataItem = new();

        // Act
        string result = columnDataItem.GetDbDataType("short");

        // Assert
        result.Should().Be("Short16");
    }

    [TestMethod]
    public void GetDataType_Time_ReturnsTimeSpan()
    {
        // Act
        string result = ColumnDataItem.GetDataType("time");

        // Assert
        result.Should().Be("TimeSpan");
    }

    [TestMethod]
    public void GetDataType_Money_ReturnsDecimal()
    {
        // Act
        string result = ColumnDataItem.GetDataType("money");

        // Assert
        result.Should().Be("decimal");
    }

    [TestMethod]
    public void GetDataType_SmallInt_ReturnsShort()
    {
        // Act
        string result = ColumnDataItem.GetDataType("smallint");

        // Assert
        result.Should().Be("short");
    }

    [TestMethod]
    public void GetDefaultPropertyValue_SmallInt_ReturnsZeroDefault()
    {
        // Act
        string result = ColumnDataItem.GetDefaultPropertyValue("smallint", string.Empty);

        // Assert
        result.Should().Be(" = 0");
    }
}
