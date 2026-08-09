using System.Collections.Generic;
using FluentAssertions;
using Konfidence.SqlHostProvider.SqlDbSchema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Konfidence.SqlHostProvider.UnitTest.SqlDbSchema;

/// <summary>
/// ColumnDataExtensions is pure list logic with no database involvement at all. The four
/// Get*FieldNames overloads had no coverage at all before - only Find and HasDefaultValueFields
/// were tested. Nothing here needs a database, so it runs anywhere the test project does.
/// </summary>
[TestClass]
public class ColumnDataExtensionsTests
{
    [TestMethod]
    public void GetJoinedFieldNames_WithMatchingNames_ConcatenatesThemWithoutASeparator()
    {
        // Arrange
        List<IColumnDataItem> columnDataItems = [CreateColumn("Id"), CreateColumn("Name"), CreateColumn("Amount")];

        // Act
        string result = columnDataItems.GetJoinedFieldNames(["Id", "Amount"]);

        // Assert
        result.Should().Be("IdAmount");
    }

    [TestMethod]
    public void GetJoinedFieldNames_MatchesCaseInsensitively()
    {
        // Arrange
        // The filter compares with OrdinalIgnoreCase, so the requested casing does not have to match
        // the column's own casing - and the *column's* spelling is what gets returned.
        List<IColumnDataItem> columnDataItems = [CreateColumn("Id"), CreateColumn("Name")];

        // Act
        string result = columnDataItems.GetJoinedFieldNames(["ID", "NAME"]);

        // Assert
        result.Should().Be("IdName");
    }

    [TestMethod]
    public void GetJoinedFieldNames_WithNoMatches_ReturnsEmptyString()
    {
        // Arrange
        List<IColumnDataItem> columnDataItems = [CreateColumn("Id")];

        // Act
        string result = columnDataItems.GetJoinedFieldNames(["DoesNotExist"]);

        // Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void GetJoinedUnderscoreFieldNames_WithMatchingNames_JoinsWithUnderscoresInUpperCase()
    {
        // Arrange
        List<IColumnDataItem> columnDataItems = [CreateColumn("Id"), CreateColumn("Name"), CreateColumn("Amount")];

        // Act
        string result = columnDataItems.GetJoinedUnderscoreFieldNames(["Id", "Name"]);

        // Assert
        result.Should().Be("ID_NAME");
    }

    [TestMethod]
    public void GetFieldNamesAsArguments_WithMatchingNames_JoinsWithCommaAndSpace()
    {
        // Arrange
        List<IColumnDataItem> columnDataItems = [CreateColumn("Id"), CreateColumn("Name")];

        // Act
        string result = columnDataItems.GetFieldNamesAsArguments(["Id", "Name"]);

        // Assert
        result.Should().Be("Id, Name");
    }

    [TestMethod]
    public void GetFieldNamesAsParameters_WithMatchingNames_PrefixesEachNameWithItsDataType()
    {
        // Arrange
        // Unlike the other three, this one projects "{DataType} {name}" and lower-cases the name,
        // so it is the only overload where DataType matters.
        List<IColumnDataItem> columnDataItems = [CreateColumn("Id", dataType: "int"), CreateColumn("Name", dataType: "string")];

        // Act
        string result = columnDataItems.GetFieldNamesAsParameters(["Id", "Name"]);

        // Assert
        result.Should().Be("int id, string name");
    }

    [TestMethod]
    public void Find_WithKnownColumnName_ReturnsThatColumn()
    {
        // Arrange
        List<IColumnDataItem> columnDataItems = [CreateColumn("Id"), CreateColumn("Name")];

        // Act
        IColumnDataItem? result = columnDataItems.Find("NAME");

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Name");
    }

    [TestMethod]
    public void Find_WithUnknownColumnName_ReturnsNull()
    {
        // Arrange
        List<IColumnDataItem> columnDataItems = [CreateColumn("Id")];

        // Act
        IColumnDataItem? result = columnDataItems.Find("DoesNotExist");

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public void HasDefaultValueFields_WithNoSpecialColumns_ReturnsFalse()
    {
        // Arrange
        List<IColumnDataItem> columnDataItems = [CreateColumn("Id"), CreateColumn("Name")];

        // Act
        bool result = columnDataItems.HasDefaultValueFields();

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void HasDefaultValueFields_WithAnAutoUpdatedColumn_ReturnsTrue()
    {
        // Arrange
        List<IColumnDataItem> columnDataItems = [CreateColumn("Id"), CreateColumn("SysUpdateTime", isAutoUpdated: true)];

        // Act
        bool result = columnDataItems.HasDefaultValueFields();

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void HasDefaultValueFields_WithAComputedColumn_ReturnsTrue()
    {
        // Arrange
        List<IColumnDataItem> columnDataItems = [CreateColumn("Total", isComputed: true)];

        // Act
        bool result = columnDataItems.HasDefaultValueFields();

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void HasDefaultValueFields_WithADefaultedColumn_ReturnsTrue()
    {
        // Arrange
        // Each of the three flags is checked separately in the same predicate, so all three need
        // their own case - a single "special column" test would leave two of them unexercised.
        List<IColumnDataItem> columnDataItems = [CreateColumn("CreatedOn", isDefaulted: true)];

        // Act
        bool result = columnDataItems.HasDefaultValueFields();

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void HasDefaultValueFields_WithNoColumnsAtAll_ReturnsFalse()
    {
        // Arrange
        List<IColumnDataItem> columnDataItems = [];

        // Act
        bool result = columnDataItems.HasDefaultValueFields();

        // Assert
        result.Should().BeFalse();
    }

    private static IColumnDataItem CreateColumn(
        string name,
        string dataType = "int",
        bool isAutoUpdated = false,
        bool isComputed = false,
        bool isDefaulted = false)
    {
        Mock<IColumnDataItem> columnDataItemMock = new();

        columnDataItemMock.Setup(x => x.Name).Returns(name);
        columnDataItemMock.Setup(x => x.DataType).Returns(dataType);
        columnDataItemMock.Setup(x => x.IsAutoUpdated).Returns(isAutoUpdated);
        columnDataItemMock.Setup(x => x.IsComputed).Returns(isComputed);
        columnDataItemMock.Setup(x => x.IsDefaulted).Returns(isDefaulted);

        return columnDataItemMock.Object;
    }
}
