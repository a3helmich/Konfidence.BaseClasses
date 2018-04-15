using System.Diagnostics.CodeAnalysis;
using DbSiteMapMenuClasses;
using Konfidence.MenuClasses.Tests.objects;
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
        #region properties

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #endregion properties

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            //IKernel kernel = new StandardKernel();

            //kernel.Bind<IDatabaseRepository>().To<DatabaseRepository>();
        }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion Additional test attributes

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
            var list = Bl.MenuDataItemList.GetListByMenuCode(1);

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

            callClassMethod.MethodTwo();
        }
    }
}
