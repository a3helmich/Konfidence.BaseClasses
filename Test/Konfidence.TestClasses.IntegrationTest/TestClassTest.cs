using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Konfidence.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestClasses;

namespace Konfidence.TestClasses.IntegrationTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass, TestCategory("MenuItem")]
    public class TestClassTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
            SqlTestToolExtensions.CopySqlSettingsToActiveConfiguration();

            SqlTestToolExtensions.CopySqlSecurityToActiveConfiguration("TestClassGenerator");
        }

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
            // arrange
            List<Dl.TestIntDataItem>? testIntDataItemList = Dl.TestIntDataItem.GetList();

            // act
            Dl.TestIntDataItem testIntDataItem = testIntDataItemList.First();

            // assert
            testIntDataItem.TestIntId.Should().NotBeEmpty();
            testIntDataItem.AutoIdField.Should().NotBeEmpty();
            testIntDataItem.GuidIdField.Should().NotBeEmpty();
        }
        [TestMethod]
        public void Constructor_WithInvalidKey_ReturnsNewItem()
        {
            // arrange
            List<Dl.TestIntDataItem>? testIntDataItemList = Dl.TestIntDataItem.GetList();
            Guid id = Guid.NewGuid();

            while (testIntDataItemList.Any(x => x.TestIntId == id))
            {
                id = Guid.NewGuid();
            }

            // act
            Dl.TestIntDataItem testIntDataItem = new(id);

            // assert
            testIntDataItem.IsNew.Should().BeTrue();
        }
    }
}
