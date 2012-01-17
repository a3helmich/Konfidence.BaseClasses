using Konfidence.BaseWebsiteClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Konfidence.BaseWebsiteClassesTest
{
    /// <summary>
    ///This is a test class for BasePageHelperTest and is intended
    ///to contain all BasePageHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BasePageHelperTest
    {
        private TestContext testContextInstance;

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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for GetCurrentPagePath
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Konfidence.BaseWebsiteClasses.dll")]
        public void GetCurrentPagePathTest()
        {
            string requestUrl = "http://localhost/sitemap/sitemap.aspx";

            string[] urlParts = null; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = BasePageHelper_Accessor.GetCurrentPagePath(urlParts);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");

            actual = BasePageHelper_Accessor.CurrentPagePath;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Konfidence.BaseWebsiteClasses.dll")]
        public void GetCurrentPageNameTest()
        {
            string requestUrl = "http://localhost/sitemap/sitemap.aspx";

            string[] urlParts = null; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = BasePageHelper_Accessor.GetCurrentPageName(urlParts);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");

            actual = BasePageHelper_Accessor.CurrentPageName;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetCurrentLanguage
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Konfidence.BaseWebsiteClasses.dll")]
        public void GetCurrentLanguageTest()
        {
            string requestUrl = "http://localhost/sitemap/sitemap.aspx";

            string[] urlParts = null; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = BasePageHelper_Accessor.GetCurrentLanguage(urlParts);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");

            actual = BasePageHelper_Accessor.CurrentLanguage;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetCurrentDomainExtension
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Konfidence.BaseWebsiteClasses.dll")]
        public void GetCurrentDomainExtensionTest()
        {
            string requestUrl = "http://localhost/sitemap/sitemap.aspx";

            string[] urlParts = null; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = BasePageHelper_Accessor.GetCurrentDomainExtension(urlParts);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");

            actual = target.CurrentDomainExtension;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetCurrentDnsName
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Konfidence.BaseWebsiteClasses.dll")]
        public void GetCurrentDnsNameTest()
        {
            string requestUrl = "http://localhost/sitemap/sitemap.aspx";

            string[] urlParts = null; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            BasePageHelper_Accessor target = new BasePageHelper_Accessor(urlParts); // TODO: Initialize to an appropriate value

            string actual;
            actual = BasePageHelper_Accessor.GetCurrentDnsName(urlParts);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");


            actual = target.CurrentDnsName;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for BasePageHelper Constructor
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Konfidence.BaseWebsiteClasses.dll")]
        public void BasePageHelperConstructorEmptyUrlTest()
        {
            string requestUrl = string.Empty; 

            BasePageHelper_Accessor target = new BasePageHelper_Accessor(requestUrl);

            Assert.AreEqual(target._UrlParts.Length, 0);
        }

        /// <summary>
        ///A test for BasePageHelper Constructor
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Konfidence.BaseWebsiteClasses.dll")]
        public void BasePageHelperConstructorUrlTest()
        {
            string requestUrl = "http://localhost/sitemap/sitemap.aspx";

            BasePageHelper_Accessor target = new BasePageHelper_Accessor(requestUrl);

            Assert.AreEqual(target._UrlParts.Length, 5);
        }
    }
}
