using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestClasses;

namespace Konfidence.TestClasses.LocalDb.UnitTest;

[TestClass, TestCategory("MenuItem")]
public class TestClassTest
{
    [TestMethod]
    public void Constructor_WithExistingId_ReturnsItem()
    {
        Dl.Test2DataItem dataItem = new(2);

        dataItem.Should().NotBeNull();
    }

    [TestMethod]
    public void Constructor_WithTwoExistingIds_ReturnsBothItems()
    {
        Dl.Test2DataItem dataItem1 = new(1);
        Dl.Test2DataItem dataItem2 = new(2);

        dataItem1.Should().NotBeNull();
        dataItem2.Should().NotBeNull();
    }

    [TestMethod]
    public void Constructor_WithParentId_ReturnsItem()
    {
        Dl.Test2DataItem test = new(1);

        test.Should().NotBeNull();
    }

    [TestMethod]
    public void GetList_ExistingData_ReturnsGuidIdField()
    {
        // Arrange
        List<Dl.TestIntDataItem>? testIntDataItemList = Dl.TestIntDataItem.GetList();

        // Act
        Dl.TestIntDataItem testIntDataItem = testIntDataItemList.First();

        // Assert
        testIntDataItem.TestIntId.Should().NotBeEmpty();
        testIntDataItem.AutoIdField.Should().NotBeEmpty();
        testIntDataItem.GuidIdField.Should().NotBeEmpty();
    }

    [TestMethod]
    public void Constructor_WithInvalidKey_ReturnsNewItem()
    {
        // Arrange
        List<Dl.TestIntDataItem>? testIntDataItemList = Dl.TestIntDataItem.GetList();
        Guid id = Guid.NewGuid();

        while (testIntDataItemList.Any(x => x.TestIntId == id))
        {
            id = Guid.NewGuid();
        }

        // Act
        Dl.TestIntDataItem testIntDataItem = new(id);

        // Assert
        testIntDataItem.IsNew.Should().BeTrue();
    }
}
