using System.Collections.Generic;
using FluentAssertions;
using Konfidence.SqlHostProvider.SqlDbSchema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Konfidence.SqlHostProvider.UnitTest.SqlDbSchema;

[TestClass]
public class ColumnDataExtensionsTests
{
    private static Mock<IColumnDataItem> CreateColumnMock(string name, bool isAutoUpdated = false, bool isComputed = false, bool isDefaulted = false)
    {
        Mock<IColumnDataItem> columnMock = new();

        columnMock.Setup(x => x.Name).Returns(name);
        columnMock.Setup(x => x.IsAutoUpdated).Returns(isAutoUpdated);
        columnMock.Setup(x => x.IsComputed).Returns(isComputed);
        columnMock.Setup(x => x.IsDefaulted).Returns(isDefaulted);

        return columnMock;
    }

    [TestMethod]
    public void Find_WithMatchingColumnName_ReturnsColumn()
    {
        // Arrange
        List<IColumnDataItem> columnDataItems = [CreateColumnMock("Naam").Object, CreateColumnMock("Omschrijving").Object];

        // Act
        IColumnDataItem? result = columnDataItems.Find("naam");

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Naam");
    }

    [TestMethod]
    public void Find_WithNoMatchingColumnName_ReturnsNull()
    {
        // Arrange
        List<IColumnDataItem> columnDataItems = [CreateColumnMock("Naam").Object];

        // Act
        IColumnDataItem? result = columnDataItems.Find("DoesNotExist");

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public void HasDefaultValueFields_WithNoSpecialColumns_ReturnsFalse()
    {
        // Arrange
        List<IColumnDataItem> columnDataItems = [CreateColumnMock("Naam").Object];

        // Act
        bool result = columnDataItems.HasDefaultValueFields();

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void HasDefaultValueFields_WithAutoUpdatedColumn_ReturnsTrue()
    {
        // Arrange
        List<IColumnDataItem> columnDataItems = [CreateColumnMock("Naam", isAutoUpdated: true).Object];

        // Act
        bool result = columnDataItems.HasDefaultValueFields();

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void HasDefaultValueFields_WithComputedColumn_ReturnsTrue()
    {
        // Arrange
        List<IColumnDataItem> columnDataItems = [CreateColumnMock("Naam", isComputed: true).Object];

        // Act
        bool result = columnDataItems.HasDefaultValueFields();

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void HasDefaultValueFields_WithDefaultedColumn_ReturnsTrue()
    {
        // Arrange
        List<IColumnDataItem> columnDataItems = [CreateColumnMock("Naam", isDefaulted: true).Object];

        // Act
        bool result = columnDataItems.HasDefaultValueFields();

        // Assert
        result.Should().BeTrue();
    }
}
