using System.Diagnostics.CodeAnalysis;
using DbMenuClasses;
using Konfidence.MenuClasses.Tests.objects;
using Konfidence.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.MenuClasses.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [ExcludeFromCodeCoverage]
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

            Assert.IsNotNull(dataItem);
        }

        [TestMethod, TestCategory("MenuItem")]
        public void GetTwoSingleMenuItem()
        {
            var dataItem1 = new Bl.MenuDataItem(1);
            var dataItem2 = new Bl.MenuDataItem(2);

            Assert.IsNotNull(dataItem1);
            Assert.IsNotNull(dataItem2);
        }

        [TestMethod, TestCategory("MenuItem")]
        public void GetSingleMenuItemList()
        {
            var list = Bl.MenuDataItemList.GetListByMenuId(1);

            Assert.AreEqual(9, list.Count, "was expecting to get 9 menu items back");

            Assert.AreEqual("Wijzigen van mijn persoonsgegevens", list[3].MenuText.MenuText, "menu text klopt niet");
        }

        [TestMethod, TestCategory("MenuItem")]
        public void GetParentMenuItem()
        {
            var test = new Bl.MenuDataItem(1);

            Assert.IsNotNull(test);
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
