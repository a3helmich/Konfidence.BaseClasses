using System;
using System.Linq;
using FluentAssertions;
using Konfidence.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestClasses;

namespace Konfidence.TestClasses.Tests
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
        public void GetSingleMenuItem()
        {
            var dataItem = new Dl.Test2DataItem(2);

            dataItem.Should().NotBeNull();
        }

        [TestMethod]
        public void GetTwoSingleMenuItem()
        {
            var dataItem1 = new Dl.Test2DataItem(1);
            var dataItem2 = new Dl.Test2DataItem(2);

            dataItem1.Should().NotBeNull();
            dataItem2.Should().NotBeNull();
        }

        [TestMethod]
        public void GetSingleMenuItemList()
        {
            var list = Dl.TestIntDataItem.GetList();

            list.Should().HaveCount(1, "list should contain 1 menu items");

            //list[3].MenuText.MenuText.Should().Be("Wijzigen van mijn persoonsgegevens");
        }

        [TestMethod]
        public void GetParentMenuItem()
        {
            var test = new Dl.Test2DataItem(1);

            test.Should().NotBeNull();
        }

        [TestMethod]
        public void When_Table_Test1_is_retrieved_and_table_does_contain_data_Should_return_GuidIdField()
        {
            // arrange
            var testIntDataItemList = Dl.TestIntDataItem.GetList();

            // act
            var testIntDataItem = testIntDataItemList.First();

            // assert
            testIntDataItem.TestIntId.Should().NotBeEmpty();
            testIntDataItem.AutoIdField.Should().NotBeEmpty();
            testIntDataItem.GuidIdField.Should().NotBeEmpty();
        }
        [TestMethod]
        public void When_Retrieving_data_with_invalid_key_Should_return_null()
        {
            // arrange
            var testIntDataItemList = Dl.TestIntDataItem.GetList();
            var id = Guid.NewGuid();

            while (testIntDataItemList.Any(x => x.TestIntId == id))
            {
                id = Guid.NewGuid();
            }

            // act
            var testIntDataItem = new Dl.TestIntDataItem(id);

            // assert
            testIntDataItem.IsNew.Should().BeTrue();
        }
    }
}
