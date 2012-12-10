using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Konfidence.BaseUserControlHelpers.DbSiteMapProvider;
using Konfidence.NewsletterClasses;

namespace MenuTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class MenuItemTest
    {
        public MenuItemTest()
        {
        }

        private TestContext testContextInstance;

        #region properties
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        #endregion properties

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
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

        [TestMethod]
        public void GetSingleMenuItem()
        {
            Bl.MenuDataItem menuItem = new Bl.MenuDataItem(2);
        }

        [TestMethod]
        public void GetSingleMenuItemList()
        {
            Bl.MenuDataItemList menuItemList = Bl.MenuDataItemList.GetListByMenuCode(1);
        }

        [TestMethod]
        public void GetParentMenuItem()
        {
            Bl.MenuDataItem menuItem = new Bl.MenuDataItem(1);
        }
    }
}
