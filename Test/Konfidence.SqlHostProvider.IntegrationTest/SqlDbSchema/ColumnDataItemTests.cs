using FluentAssertions;
using Konfidence.SqlHostProvider.SqlDbSchema;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.SqlHostProvider.IntegrationTest.SqlDbSchema;

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
}
