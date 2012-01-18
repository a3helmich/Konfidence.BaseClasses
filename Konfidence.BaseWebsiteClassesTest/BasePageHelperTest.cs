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

        #region UrlRequestTest

        [TestMethod()]
        public void UrlRequest00Test()
        {
            BasePageHelper helper = null;
            string urlRequest = string.Empty;

            helper = new BasePageHelper(urlRequest);
            Assert.IsFalse(helper.IsValid, "empty url");
        }

        [TestMethod()]
        public void UrlRequest01Test()
        {
            BasePageHelper helper = null;
            string urlRequest = "http://localhost";

            helper = new BasePageHelper(urlRequest);
            Assert.IsTrue(helper.IsValid, urlRequest);
        }

        [TestMethod()]
        public void UrlRequest02Test()
        {
            BasePageHelper helper = null;
            string urlRequest = "http://localhost/";

            helper = new BasePageHelper(urlRequest);
            Assert.IsTrue(helper.IsValid, urlRequest);
        }

        [TestMethod()]
        public void UrlRequest03Test()
        {
            BasePageHelper helper = null;
            string urlRequest = "http://localhost:8080";

            helper = new BasePageHelper(urlRequest);
            Assert.IsTrue(helper.IsValid, urlRequest);
        }

        [TestMethod()]
        public void UrlRequest04Test()
        {
            BasePageHelper helper = null;
            string urlRequest = "http://localhost:8080/";

            helper = new BasePageHelper(urlRequest);
            Assert.IsTrue(helper.IsValid, urlRequest);
        }

        [TestMethod()]
        public void UrlRequest05Test()
        {
            BasePageHelper helper = null;
            string urlRequest = "http://localhost/sitemap.aspx";

            helper = new BasePageHelper(urlRequest);
            Assert.IsTrue(helper.IsValid, urlRequest);
        }

        [TestMethod()]
        public void UrlRequest06Test()
        {
            BasePageHelper helper = null;
            string urlRequest = "http://localhost:8080/sitemap.aspx";

            helper = new BasePageHelper(urlRequest);
            Assert.IsTrue(helper.IsValid, urlRequest);
        }

        [TestMethod()]
        public void UrlRequest07Test()
        {
            BasePageHelper helper = null;
            string urlRequest = "http://localhost/sitemap/sitemap.aspx";

            helper = new BasePageHelper(urlRequest);
            Assert.IsTrue(helper.IsValid, urlRequest);
        }

        [TestMethod()]
        public void UrlRequest08Test()
        {
            BasePageHelper helper = null;
            string urlRequest = "http://localhost:8080/sitemap/sitemap.aspx";

            helper = new BasePageHelper(urlRequest);
            Assert.IsTrue(helper.IsValid, urlRequest);
        }

        [TestMethod()]
        public void UrlRequest09Test()
        {
            BasePageHelper helper = null;
            string urlRequest = "http://localhost/sitemap/h/h/h/h/h/sitemap.aspx";

            helper = new BasePageHelper(urlRequest);
            Assert.IsTrue(helper.IsValid, urlRequest);
        }

        [TestMethod()]
        public void UrlRequest10Test()
        {
            BasePageHelper helper = null;
            string urlRequest = "http://localhost:8080/sitemap/h/h/h/h/h/sitemap.aspx";

            helper = new BasePageHelper(urlRequest);
            Assert.IsTrue(helper.IsValid, urlRequest);
        }

        #endregion UrlRequestTest

        /// <summary>
        ///A test for GetCurrentPagePath
        ///</summary>
        [TestMethod()]
        public void GetCurrentPagePath01Test()
        {
            string requestUrl = "http://localhost/sitemap/sitemap.aspx";

            BasePageHelper target = new BasePageHelper(requestUrl);

            string expected = "/sitemap/sitemap.aspx";

            Assert.AreEqual(expected, target.CurrentPagePath);
        }

        /// <summary>
        ///A test for GetCurrentPagePath
        ///</summary>
        [TestMethod()]
        public void GetCurrentPagePath02Test()
        {
            string requestUrl = "http://www.konfidence.nl/sitemap/sitemap.aspx";

            BasePageHelper target = new BasePageHelper(requestUrl);

            string expected = "/sitemap/sitemap.aspx";

            Assert.AreEqual(expected, target.CurrentPagePath);
        }

        /// <summary>
        ///A test for GetCurrentPagePath
        ///</summary>
        [TestMethod()]
        public void GetCurrentPagePath03Test()
        {
            string requestUrl = "http://www.konfidence.nl";

            BasePageHelper target = new BasePageHelper(requestUrl);

            string expected = "/";

            Assert.AreEqual(expected, target.CurrentPagePath);
        }

        /// <summary>
        ///A test for GetCurrentPagePath
        ///</summary>
        [TestMethod()]
        public void GetCurrentPagePath04Test()
        {
            string requestUrl = "http://www.konfidence.nl/sitemap/h/h/h/h/h/h/h/sitemap.aspx";

            BasePageHelper target = new BasePageHelper(requestUrl);

            string expected = "/sitemap/h/h/h/h/h/h/h/sitemap.aspx";

            Assert.AreEqual(expected, target.CurrentPagePath);
        }

        #region GetCurrentPageNameTest

        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod()]
        public void GetCurrentPageName01Test()
        {
            string requestUrl = "http://localhost/sitemap/sitemap.aspx";

            BasePageHelper target = new BasePageHelper(requestUrl);

            string expected = "sitemap.aspx"; 

            Assert.AreEqual(expected, target.CurrentPageName);
        }

        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod()]
        public void GetCurrentPageName02Test()
        {
            string requestUrl = "http://localhost:8080/sitemap/sitemap.aspx";

            BasePageHelper target = new BasePageHelper(requestUrl);

            string expected = "sitemap.aspx";

            Assert.AreEqual(expected, target.CurrentPageName);
        }

        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod()]
        public void GetCurrentPageName03Test()
        {
            string requestUrl = "http://localhost/sitemap.aspx";

            BasePageHelper target = new BasePageHelper(requestUrl);

            string expected = "sitemap.aspx";

            Assert.AreEqual(expected, target.CurrentPageName);
        }

        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod()]
        public void GetCurrentPageName04Test()
        {
            string requestUrl = "http://localhost:8080/sitemap.aspx";

            BasePageHelper target = new BasePageHelper(requestUrl);

            string expected = "sitemap.aspx";

            Assert.AreEqual(expected, target.CurrentPageName);
        }

        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod()]
        public void GetCurrentPageName05Test()
        {
            string requestUrl = "http://localhost/";

            BasePageHelper target = new BasePageHelper(requestUrl);

            string expected = string.Empty;

            Assert.AreEqual(expected, target.CurrentPageName);
        }

        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod()]
        public void GetCurrentPageName06Test()
        {
            string requestUrl = "http://localhost:8080/";

            BasePageHelper target = new BasePageHelper(requestUrl);

            string expected = string.Empty;

            Assert.AreEqual(expected, target.CurrentPageName);
        }

        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod()]
        public void GetCurrentPageName07Test()
        {
            string requestUrl = "http://localhost";

            BasePageHelper target = new BasePageHelper(requestUrl);

            string expected = string.Empty;

            Assert.AreEqual(expected, target.CurrentPageName);
        }

        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod()]
        public void GetCurrentPageName08Test()
        {
            string requestUrl = "http://localhost:8080";

            BasePageHelper target = new BasePageHelper(requestUrl);

            string expected = string.Empty;

            Assert.AreEqual(expected, target.CurrentPageName);
        }

        #endregion GetCurrentPageNameTest

        #region GetCurrentLanguageTest

        /// <summary>
        ///A test for GetCurrentLanguage
        ///</summary>
        [TestMethod()]
        public void GetCurrentLanguage01Test()
        {
            string requestUrl = "http://localhost/sitemap/sitemap.aspx";

            BasePageHelper target = new BasePageHelper(requestUrl);

            string expected = "nl";

            Assert.AreEqual(expected, target.CurrentLanguage);
        }

        /// <summary>
        ///A test for GetCurrentLanguage
        ///</summary>
        [TestMethod()]
        public void GetCurrentLanguage02Test()
        {
            string requestUrl = "http://www.konfidence.nl/sitemap/sitemap.aspx";

            BasePageHelper target = new BasePageHelper(requestUrl);

            string expected = "nl";

            Assert.AreEqual(expected, target.CurrentLanguage);
        }

        /// <summary>
        ///A test for GetCurrentLanguage
        ///</summary>
        [TestMethod()]
        public void GetCurrentLanguage03Test()
        {
            string requestUrl = "http://www.konfidence.be/sitemap/sitemap.aspx";

            BasePageHelper target = new BasePageHelper(requestUrl);

            string expected = "nl";

            Assert.AreEqual(expected, target.CurrentLanguage);
        }

        /// <summary>
        ///A test for GetCurrentLanguage
        ///</summary>
        [TestMethod()]
        public void GetCurrentLanguage04Test()
        {
            string requestUrl = "http://www.konfidence.eu/sitemap/sitemap.aspx";

            BasePageHelper target = new BasePageHelper(requestUrl);

            string expected = "uk";

            Assert.AreEqual(expected, target.CurrentLanguage);
        }

        /// <summary>
        ///A test for GetCurrentLanguage
        ///</summary>
        [TestMethod()]
        public void GetCurrentLanguage05Test()
        {
            string requestUrl = "http://www.konfidence.co.uk/sitemap/sitemap.aspx";

            BasePageHelper target = new BasePageHelper(requestUrl);

            string expected = "uk";

            Assert.AreEqual(expected, target.CurrentLanguage);
        }

        /// <summary>
        ///A test for GetCurrentLanguage
        ///</summary>
        [TestMethod()]
        public void GetCurrentLanguage06Test()
        {
            string requestUrl = "http://www.konfidence.fr/sitemap/sitemap.aspx";

            BasePageHelper target = new BasePageHelper(requestUrl);

            string expected = "fr";

            Assert.AreEqual(expected, target.CurrentLanguage);
        }

        #endregion GetCurrentLanguageTest

        #region GetCurrentDomainExtensionTest

        /// <summary>
        ///A test for GetCurrentDomainExtension
        ///</summary>
        [TestMethod()]
        public void GetCurrentDomainExtension01Test()
        {
            string requestUrl = "http://localhost/sitemap/sitemap.aspx";

            BasePageHelper target = new BasePageHelper(requestUrl); // TODO: Initialize to an appropriate value

            string expected = "nl";
            
            Assert.AreEqual(expected, target.CurrentDomainExtension);
        }

        /// <summary>
        ///A test for GetCurrentDomainExtension
        ///</summary>
        [TestMethod()]
        public void GetCurrentDomainExtension02Test()
        {
            string requestUrl = "http://www.konfidence.nl/sitemap/sitemap.aspx";

            BasePageHelper target = new BasePageHelper(requestUrl); // TODO: Initialize to an appropriate value

            string expected = "nl";

            Assert.AreEqual(expected, target.CurrentDomainExtension);
        }

        /// <summary>
        ///A test for GetCurrentDomainExtension
        ///</summary>
        [TestMethod()]
        public void GetCurrentDomainExtension03Test()
        {
            string requestUrl = "http://www.konfidence.eu/sitemap/sitemap.aspx";

            BasePageHelper target = new BasePageHelper(requestUrl); // TODO: Initialize to an appropriate value

            string expected = "eu";

            Assert.AreEqual(expected, target.CurrentDomainExtension);
        }

        /// <summary>
        ///A test for GetCurrentDomainExtension
        ///</summary>
        [TestMethod()]
        public void GetCurrentDomainExtension04Test()
        {
            string requestUrl = "http://www.konfidence.co.uk/sitemap/sitemap.aspx";

            BasePageHelper target = new BasePageHelper(requestUrl); // TODO: Initialize to an appropriate value

            string expected = "co.uk";

            Assert.AreEqual(expected, target.CurrentDomainExtension);
        }

        /// <summary>
        ///A test for GetCurrentDomainExtension
        ///</summary>
        [TestMethod()]
        public void GetCurrentDomainExtension05Test()
        {
            string requestUrl = "http://www.konfidence.gov.uk/sitemap/sitemap.aspx";

            BasePageHelper target = new BasePageHelper(requestUrl); // TODO: Initialize to an appropriate value

            string expected = "gov.uk";

            Assert.AreEqual(expected, target.CurrentDomainExtension);
        }

        #endregion GetCurrentDomainExtensionTest

        #region GetCurrentDnsNameTest

        /// <summary>
        ///A test for GetCurrentDnsName
        ///</summary>
        [TestMethod()]
        public void GetCurrentDnsName01Test()
        {
            string requestUrl = "http://localhost/sitemap/sitemap.aspx";

            BasePageHelper target = new BasePageHelper(requestUrl); // TODO: Initialize to an appropriate value

            string expected = "www.konfidence.nl";

            Assert.AreEqual(expected, target.CurrentDnsName);
        }

        /// <summary>
        ///A test for GetCurrentDnsName
        ///</summary>
        [TestMethod()]
        public void GetCurrentDnsName02Test()
        {
            string requestUrl = "http://www.konfidence.nl/sitemap/sitemap.aspx";

            BasePageHelper target = new BasePageHelper(requestUrl); // TODO: Initialize to an appropriate value

            string expected = "www.konfidence.nl";

            Assert.AreEqual(expected, target.CurrentDnsName);
        }

        /// <summary>
        ///A test for GetCurrentDnsName
        ///</summary>
        [TestMethod()]
        public void GetCurrentDnsName03Test()
        {
            string requestUrl = "http://www.konfidence.co.uk/sitemap/sitemap.aspx";

            BasePageHelper target = new BasePageHelper(requestUrl); // TODO: Initialize to an appropriate value

            string expected = "www.konfidence.co.uk";

            Assert.AreEqual(expected, target.CurrentDnsName);
        }

        #endregion GetCurrentDnsNameTest
    }
}
