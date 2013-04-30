﻿using DbSiteMapMenuClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MenuTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
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
            var dataItem = new Bl.MenuDataItem(2);

            Assert.IsNotNull(dataItem);
        }

        [TestMethod]
        public void GetSingleMenuItemList()
        {
            Bl.MenuDataItemList.GetListByMenuCode(1);
        }

        [TestMethod]
        public void GetParentMenuItem()
        {
            var test = new Bl.MenuDataItem(1);

            Assert.IsNotNull(test);
        }
    }
}