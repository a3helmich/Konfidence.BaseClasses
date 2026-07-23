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
    public void Find_With_matching_column_name_Should_return_column()
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
    public void Find_With_no_matching_column_name_Should_return_null()
    {
        // Arrange
        List<IColumnDataItem> columnDataItems = [CreateColumnMock("Naam").Object];

        // Act
        IColumnDataItem? result = columnDataItems.Find("DoesNotExist");

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public void HasDefaultValueFields_With_no_special_columns_Should_return_false()
    {
        // Arrange
        List<IColumnDataItem> columnDataItems = [CreateColumnMock("Naam").Object];

        // Act
        bool result = columnDataItems.HasDefaultValueFields();

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void HasDefaultValueFields_With_autoUpdated_column_Should_return_true()
    {
        // Arrange
        List<IColumnDataItem> columnDataItems = [CreateColumnMock("Naam", isAutoUpdated: true).Object];

        // Act
        bool result = columnDataItems.HasDefaultValueFields();

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void HasDefaultValueFields_With_computed_column_Should_return_true()
    {
        // Arrange
        List<IColumnDataItem> columnDataItems = [CreateColumnMock("Naam", isComputed: true).Object];

        // Act
        bool result = columnDataItems.HasDefaultValueFields();

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void HasDefaultValueFields_With_defaulted_column_Should_return_true()
    {
        // Arrange
        List<IColumnDataItem> columnDataItems = [CreateColumnMock("Naam", isDefaulted: true).Object];

        // Act
        bool result = columnDataItems.HasDefaultValueFields();

        // Assert
        result.Should().BeTrue();
    }
}
