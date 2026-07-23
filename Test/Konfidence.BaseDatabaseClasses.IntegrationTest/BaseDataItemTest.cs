using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Konfidence.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestClasses;

namespace Konfidence.BaseDatabaseClasses.IntegrationTest
{
    [TestClass]
    public class BaseDataItemTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
            SqlTestToolExtensions.CopySqlSettingsToActiveConfiguration();

            SqlTestToolExtensions.CopySqlSecurityToActiveConfiguration("TestClassGenerator");
        }

        [TestInitialize]
        public void TestInitialize()
        {
            List<Dl.TestIntDataItem> testIntDataItemList = Dl.TestIntDataItem
                .GetList()
                .Where(x => x.GetId() > 1)
                .ToList();

            testIntDataItemList.ForEach(item => item.Delete());
        }

        [TestCleanup]
        public void TestCleanup()
        {
            List<Dl.TestIntDataItem> testIntDataItemList = Dl.TestIntDataItem
                .GetList()
                .Where(x => x.GetId() > 1)
                .ToList();

            testIntDataItemList.ForEach(item => item.Delete());
        }

        [TestMethod]
        public void GetList_ExistingItem_ReturnsShortAndLongFields()
        {
            // Arrange

            // Act
            List<Dl.TestIntDataItem>? testIntDataItemList = Dl.TestIntDataItem.GetList();

            // Assert
            testIntDataItemList.Should().HaveCount(1);
            testIntDataItemList[0].testInt.Should().BeGreaterThan(1);

            testIntDataItemList[0].testTinyInt.Should().Be(10);
            testIntDataItemList[0].testInt.Should().Be(1000);
            testIntDataItemList[0].testBigInt.Should().Be(100);
        }

        [TestMethod]
        public void Constructor_WithSavedItem_ReturnsQueriedItem()
        {
            // Arrange
            Dl.TestIntDataItem testIntDataItem = new()
            {
                testTinyInt = 111, 
                testInt = 1111, 
                testBigInt = 11111
            };

            testIntDataItem.Save();

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
            Dl.TestIntDataItem testIntDataItem = new()
            {
                testTinyInt = 11, 
                testInt = 1111, 
                testBigInt = 11111
            };

            testIntDataItem.Save();

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
            Dl.TestIntDataItem testIntDataItem = new()
            {
                testTinyInt = 11,
                testInt = 1111,
                testBigInt = 11111
            };

            testIntDataItem.Save();

            // Act
            Dl.TestIntDataItem updateTestIntDataItem = new(testIntDataItem.GetId());
            Dl.TestIntDataItem updateTestIntGuidDataItem = new(updateTestIntDataItem.TestIntId);

            // Assert
            updateTestIntGuidDataItem.TestIntId.Should().Be(updateTestIntDataItem.TestIntId);
            updateTestIntGuidDataItem.GetId().Should().Be(updateTestIntDataItem.GetId());
        }
    }
}
