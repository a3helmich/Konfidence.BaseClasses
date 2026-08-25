using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestClasses;

namespace Konfidence.BaseDatabaseClasses.LocalDb.UnitTest;

[TestClass]
public class BaseDataItemTest
{
    // Kept in step with the IntegrationTest copy of this fixture, which only ever touches rows it
    // created itself: deleting every row with Id > 1 also deletes rows another concurrently running
    // test host still has in flight, and asserting on the row count fails whenever another host has
    // one open. LocalDbTestDatabase gives each process its own attached database, so this copy is
    // not exposed to that today - but the two fixtures are otherwise identical, and the trap would
    // re-arm the moment this one is pointed at a shared database.
    private const int BaselineId = 1;

    private readonly List<Dl.TestIntDataItem> _createdItems = [];

    [TestCleanup]
    public void TestCleanup()
    {
        _createdItems.ForEach(item => item.Delete());

        _createdItems.Clear();
    }

    [TestMethod]
    public void GetList_ExistingItem_ReturnsShortAndLongFields()
    {
        // Arrange

        // Act
        List<Dl.TestIntDataItem> testIntDataItemList = Dl.TestIntDataItem.GetList();

        // Assert
        Dl.TestIntDataItem baselineItem = testIntDataItemList.Single(x => x.GetId() == BaselineId);

        baselineItem.testTinyInt.Should().Be(10);
        baselineItem.testInt.Should().Be(1000);
        baselineItem.testBigInt.Should().Be(100);
    }

    [TestMethod]
    public void Constructor_WithSavedItem_ReturnsQueriedItem()
    {
        // Arrange
        Dl.TestIntDataItem testIntDataItem = SaveNewItem(111, 1111, 11111);

        // Act
        Dl.TestIntDataItem copyTestIntDataItem = new(testIntDataItem.GetId());

        // Assert
        copyTestIntDataItem.GetId().Should().Be(testIntDataItem.GetId());
        copyTestIntDataItem.TestIntId.Should().Be(testIntDataItem.TestIntId);

        copyTestIntDataItem.testTinyInt.Should().Be(111);
        copyTestIntDataItem.testInt.Should().Be(1111);
        copyTestIntDataItem.testBigInt.Should().Be(11111);
    }

    [TestMethod]
    public void Constructor_WithUpdatedItem_ReturnsUpdatedItem()
    {
        // Arrange
        Dl.TestIntDataItem testIntDataItem = SaveNewItem(11, 1111, 11111);

        Dl.TestIntDataItem copyTestIntDataItem = new(testIntDataItem.GetId())
        {
            testTinyInt = 222,
            testInt = 2222,
            testBigInt = 22222
        };

        copyTestIntDataItem.Save();

        // Act
        Dl.TestIntDataItem updateTestIntDataItem = new(testIntDataItem.GetId());

        // Assert
        updateTestIntDataItem.TestIntId.Should().Be(testIntDataItem.TestIntId);
        updateTestIntDataItem.GetId().Should().Be(testIntDataItem.GetId());

        updateTestIntDataItem.testTinyInt.Should().Be(222);
        updateTestIntDataItem.testInt.Should().Be(2222);
        updateTestIntDataItem.testBigInt.Should().Be(22222);
    }

    [TestMethod]
    public void Constructor_WithGuidId_ReturnsMatchingItem()
    {
        // Arrange
        Dl.TestIntDataItem testIntDataItem = SaveNewItem(11, 1111, 11111);

        // Act
        Dl.TestIntDataItem updateTestIntDataItem = new(testIntDataItem.GetId());
        Dl.TestIntDataItem updateTestIntGuidDataItem = new(updateTestIntDataItem.TestIntId);

        // Assert
        updateTestIntGuidDataItem.TestIntId.Should().Be(updateTestIntDataItem.TestIntId);
        updateTestIntGuidDataItem.GetId().Should().Be(updateTestIntDataItem.GetId());
    }

    private Dl.TestIntDataItem SaveNewItem(byte testTinyInt, int testInt, long testBigInt)
    {
        Dl.TestIntDataItem testIntDataItem = new()
        {
            testTinyInt = testTinyInt,
            testInt = testInt,
            testBigInt = testBigInt
        };

        testIntDataItem.Save();

        _createdItems.Add(testIntDataItem);

        return testIntDataItem;
    }
}
