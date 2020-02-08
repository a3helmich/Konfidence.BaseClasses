using System.Diagnostics.CodeAnalysis;
using DbMenuClasses;
using FluentAssertions;
using Konfidence.MenuClasses.Tests.objects;
using Konfidence.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.MenuClasses.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class MenuItemTest
    {
        [TestInitialize]
        public void initialize()
        {
            SqlTestToolExtensions.CopySqlSettingsToActiveConfiguration();
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
            var list = Bl.MenuDataItemList.GetListByMenuId(1);

            list.Should().HaveCount(9, "list should contain 9 menu items");

            list[3].MenuText.MenuText.Should().Be("Wijzigen van mijn persoonsgegevens");
        }

        [TestMethod, TestCategory("MenuItem")]
        public void GetParentMenuItem()
        {
            var test = new Bl.MenuDataItem(1);

            test.Should().NotBeNull();
        }

        [TestMethod, TestCategory("Ninject")]
        public void TestNinject()
        {
            var callClassMethod = new CallTestItemListClass();

            callClassMethod.MethodOne();
            callClassMethod.MethodTwo();
        }
    }
}
