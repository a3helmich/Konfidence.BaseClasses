using System.Linq;
using DbMenuClasses;
using FluentAssertions;
using Konfidence.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Konfidence.BaseData;

namespace Konfidence.MenuClasses.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class MenuItemTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
            SqlTestToolExtensions.CopySqlSettingsToActiveConfiguration();

            SqlTestToolExtensions.CopySqlSecurityToActiveConfiguration("DbMenu");
        }

        [TestMethod, TestCategory("MenuItem")]
        public void GetSingleMenuItem()
        {
            var dataItem = new Bl.MenuDataItem(2);

            dataItem.Should().NotBeNull();
        }

        [TestMethod, TestCategory("MenuItem")]
        public void GetTwoSingleMenuItem()
        {
            var dataItem1 = new Bl.MenuDataItem(1);
            var dataItem2 = new Bl.MenuDataItem(2);

            dataItem1.Should().NotBeNull();
            dataItem2.Should().NotBeNull();
        }

        [TestMethod, TestCategory("MenuItem")]
        public void GetSingleMenuItemList()
        {
            var list = Bl.MenuDataItem.GetListByMenuId(1);

            list.Should().HaveCount(9, "list should contain 9 menu items");

            list[3].MenuText.MenuText.Should().Be("Wijzigen van mijn persoonsgegevens");
        }

        [TestMethod, TestCategory("MenuItem")]
        public void GetSingleMenuItemByFindId()
        {
            var list = Bl.MenuDataItem.GetListByMenuId(1);

            var itemById = list.FindById("2");

            itemById?.NodeId.Should().Be(2);
        }

        [TestMethod, TestCategory("MenuItem")]
        public void GetSingleMenuItemByFindIsSelected()
        {
            var list = Bl.MenuDataItem.GetListByMenuId(1);

            var itemById = list.FindByIsSelected();

            itemById?.MenuId.Should().Be(1);
        }

        [TestMethod, TestCategory("MenuItem")]
        public void GetSingleMenuItemByFindIsEditing()
        {
            var list = Bl.MenuDataItem.GetListByMenuId(1);

            var itemById = list.FindByIsEditing();

            itemById?.MenuId.Should().Be(1);
        }

        [TestMethod, TestCategory("MenuItem")]
        public void GetSingleMenuItemByFindCurrent()
        {
            var list = Bl.MenuDataItem.GetListByMenuId(1);

            var itemById = list.FindCurrent();

            itemById?.MenuId.Should().Be(1);
        }

        [TestMethod, TestCategory("MenuItem")]
        public void GetSingleMenuItemHasCurrent()
        {
            var list = Bl.MenuDataItem.GetListByMenuId(1);

            var _ = list.FindCurrent();

            var itemHasCurrent = list.HasCurrent();

            itemHasCurrent.Should().BeTrue();
        }

        [TestMethod, TestCategory("MenuItem")]
        public void GetSingleMenuItemHasCurrentMenuId2()
        {
            var list = Bl.MenuDataItem.GetListByMenuId(2);

            var _ = list.FindCurrent();

            var itemHasCurrent = list.HasCurrent();

            itemHasCurrent.Should().BeFalse();
        }

        [TestMethod, TestCategory("MenuItem")]
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
