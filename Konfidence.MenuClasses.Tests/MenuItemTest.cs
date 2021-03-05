using System.Linq;
using DbMenuClasses;
using FluentAssertions;
using Konfidence.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.MenuClasses.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass, TestCategory("MenuItem")]
    public class MenuItemTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
            SqlTestToolExtensions.CopySqlSettingsToActiveConfiguration();

            SqlTestToolExtensions.CopySqlSecurityToActiveConfiguration("DbMenu");
        }

        [TestMethod]
        public void GetSingleMenuItem()
        {
            var dataItem = new Bl.MenuDataItem(2);

            dataItem.Should().NotBeNull();
        }

        [TestMethod]
        public void GetTwoSingleMenuItem()
        {
            var dataItem1 = new Bl.MenuDataItem(1);
            var dataItem2 = new Bl.MenuDataItem(2);

            dataItem1.Should().NotBeNull();
            dataItem2.Should().NotBeNull();
        }

        [TestMethod]
        public void GetSingleMenuItemList()
        {
            var list = Bl.MenuDataItem.GetListByMenuId(1);

            list.Should().HaveCount(9, "list should contain 9 menu items");

            list[3].MenuText.MenuText.Should().Be("Wijzigen van mijn persoonsgegevens");
        }

        [TestMethod]
        public void GetParentMenuItem()
        {
            var test = new Bl.MenuDataItem(1);

            test.Should().NotBeNull();
        }

        [TestMethod]
        public void When_Table_Test1_is_retrieved_and_table_does_contain_data_Should_return_GuidIdField()
        {
            // arrange
            var testIntDataItemList = Bl.TestIntDataItem.GetList();

            // act
            var testIntDataItem = testIntDataItemList.First();

            // assert
            testIntDataItem.TestIntId.Should().NotBeEmpty();
            testIntDataItem.AutoIdField.Should().NotBeEmpty();
            testIntDataItem.GuidIdField.Should().NotBeEmpty();
        }
    }
}
