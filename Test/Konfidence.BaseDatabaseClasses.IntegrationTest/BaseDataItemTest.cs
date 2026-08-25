using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Konfidence.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestClasses;

namespace Konfidence.BaseDatabaseClasses.IntegrationTest;

[TestClass]
public class BaseDataItemTest
{
    [ClassInitialize]
    public static void ClassInitialize(TestContext _)
    {
        SqlTestToolExtensions.CopySqlSettingsToActiveConfiguration();

        SqlTestToolExtensions.CopySqlSecurityToActiveConfiguration("TestClassGenerator");
    }

    // The TestInt table lives in the shared TestClassGenerator database, and the net9.0 and net10.0
    // test hosts can run as concurrent processes under some runners. So this fixture only ever
    // touches rows it created itself: deleting every row with Id > 1 - as it used to - also deleted
    // rows another host still had in flight, and asserting on the row count failed whenever another
    // host happened to have one open.
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
