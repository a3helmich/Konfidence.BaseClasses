using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseUserControlHelpers.Tests
{
    /// <summary>
    ///This is a test class for BasePageHelperTest and is intended
    ///to contain all BasePageHelperTest Unit Tests
    ///</summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class BasePageHelperTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region UrlRequestTest

        [TestMethod, TestCategory("UrlRequest")]
        public void UrlRequest00Test()
        {
            var urlRequest = string.Empty;

            var helper = new BasePageHelper(urlRequest, string.Empty);

            Assert.IsFalse(helper.IsValid, "empty url");
        }

        [TestMethod, TestCategory("UrlRequest")]
        public void UrlRequest01Test()
        {
            const string urlRequest = "http://localhost";

            var helper = new BasePageHelper(urlRequest, string.Empty);

            Assert.IsTrue(helper.IsValid, urlRequest);
        }

        [TestMethod, TestCategory("UrlRequest")]
        public void UrlRequest02Test()
        {
            const string urlRequest = "http://localhost/";

            var helper = new BasePageHelper(urlRequest, string.Empty);

            Assert.IsTrue(helper.IsValid, urlRequest);
        }

        [TestMethod, TestCategory("UrlRequest")]
        public void UrlRequest03Test()
        {
            const string urlRequest = "http://localhost:8080";

            var helper = new BasePageHelper(urlRequest, string.Empty);

            Assert.IsTrue(helper.IsValid, urlRequest);
        }

        [TestMethod, TestCategory("UrlRequest")]
        public void UrlRequest04Test()
        {
            const string urlRequest = "http://localhost:8080/";

            var helper = new BasePageHelper(urlRequest, string.Empty);

            Assert.IsTrue(helper.IsValid, urlRequest);
        }

        [TestMethod, TestCategory("UrlRequest")]
        public void UrlRequest05Test()
        {
            const string urlRequest = "http://localhost/sitemap.aspx";

            var helper = new BasePageHelper(urlRequest, string.Empty);

            Assert.IsTrue(helper.IsValid, urlRequest);
        }

        [TestMethod, TestCategory("UrlRequest")]
        public void UrlRequest06Test()
        {
            const string urlRequest = "http://localhost:8080/sitemap.aspx";

            var helper = new BasePageHelper(urlRequest, string.Empty);

            Assert.IsTrue(helper.IsValid, urlRequest);
        }

        [TestMethod, TestCategory("UrlRequest")]
        public void UrlRequest07Test()
        {
            const string urlRequest = "http://localhost/sitemap/sitemap.aspx";

            var helper = new BasePageHelper(urlRequest, string.Empty);

            Assert.IsTrue(helper.IsValid, urlRequest);
        }

        [TestMethod, TestCategory("UrlRequest")]
        public void UrlRequest08Test()
        {
            const string urlRequest = "http://localhost:8080/sitemap/sitemap.aspx";

            var helper = new BasePageHelper(urlRequest, string.Empty);

            Assert.IsTrue(helper.IsValid, urlRequest);
        }

        [TestMethod, TestCategory("UrlRequest")]
        public void UrlRequest09Test()
        {
            const string urlRequest = "http://localhost/sitemap/h/h/h/h/h/sitemap.aspx";

            var helper = new BasePageHelper(urlRequest, string.Empty);

            Assert.IsTrue(helper.IsValid, urlRequest);
        }

        [TestMethod, TestCategory("UrlRequest")]
        public void UrlRequest10Test()
        {
            const string urlRequest = "http://localhost:8080/sitemap/h/h/h/h/h/sitemap.aspx";

            var helper = new BasePageHelper(urlRequest, string.Empty);

            Assert.IsTrue(helper.IsValid, urlRequest);
        }

        #endregion UrlRequestTest

        /// <summary>
        ///A test for GetCurrentPagePath
        ///</summary>
        [TestMethod, TestCategory("CurrentPagePath")]
        public void GetCurrentPagePath01Test()
        {
            const string requestUrl = "http://localhost/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "/sitemap/sitemap.aspx";

            Assert.AreEqual(expected, target.CurrentPagePath);
        }

        /// <summary>
        ///A test for GetCurrentPagePath
        ///</summary>
        [TestMethod, TestCategory("CurrentPagePath")]
        public void GetCurrentPagePath02Test()
        {
            const string requestUrl = "http://www.konfidence.nl/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "/sitemap/sitemap.aspx";

            Assert.AreEqual(expected, target.CurrentPagePath);
        }

        /// <summary>
        ///A test for GetCurrentPagePath
        ///</summary>
        [TestMethod, TestCategory("CurrentPagePath")]
        public void GetCurrentPagePath03Test()
        {
            const string requestUrl = "http://www.konfidence.nl";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "/";

            Assert.AreEqual(expected, target.CurrentPagePath);
        }

        /// <summary>
        ///A test for GetCurrentPagePath
        ///</summary>
        [TestMethod, TestCategory("CurrentPagePath")]
        public void GetCurrentPagePath04Test()
        {
            const string requestUrl = "http://www.konfidence.nl/sitemap/h/h/h/h/h/h/h/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "/sitemap/h/h/h/h/h/h/h/sitemap.aspx";

            Assert.AreEqual(expected, target.CurrentPagePath);
        }

        #region GetCurrentPageNameTest

        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod, TestCategory("CurrentPageName")]
        public void GetCurrentPageName01Test()
        {
            const string requestUrl = "http://localhost/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "sitemap.aspx"; 

            Assert.AreEqual(expected, target.CurrentPageName);
        }

        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod, TestCategory("CurrentPageName")]
        public void GetCurrentPageName02Test()
        {
            const string requestUrl = "http://localhost:8080/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "sitemap.aspx";

            Assert.AreEqual(expected, target.CurrentPageName);
        }

        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod, TestCategory("CurrentPageName")]
        public void GetCurrentPageName03Test()
        {
            const string requestUrl = "http://localhost/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "sitemap.aspx";

            Assert.AreEqual(expected, target.CurrentPageName);
        }

        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod, TestCategory("CurrentPageName")]
        public void GetCurrentPageName04Test()
        {
            const string requestUrl = "http://localhost:8080/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "sitemap.aspx";

            Assert.AreEqual(expected, target.CurrentPageName);
        }

        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod, TestCategory("CurrentPageName")]
        public void GetCurrentPageName05Test()
        {
            const string requestUrl = "http://localhost/";

            var target = new BasePageHelper(requestUrl, string.Empty);

            var expected = string.Empty;

            Assert.AreEqual(expected, target.CurrentPageName);
        }

        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod, TestCategory("CurrentPageName")]
        public void GetCurrentPageName06Test()
        {
            const string requestUrl = "http://localhost:8080/";

            var target = new BasePageHelper(requestUrl, string.Empty);

            var expected = string.Empty;

            Assert.AreEqual(expected, target.CurrentPageName);
        }

        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod, TestCategory("CurrentPageName")]
        public void GetCurrentPageName07Test()
        {
            const string requestUrl = "http://localhost";

            var target = new BasePageHelper(requestUrl, string.Empty);

            var expected = string.Empty;

            Assert.AreEqual(expected, target.CurrentPageName);
        }

        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod, TestCategory("CurrentPageName")]
        public void GetCurrentPageName08Test()
        {
            const string requestUrl = "http://localhost:8080";

            var target = new BasePageHelper(requestUrl, string.Empty);

            var expected = string.Empty;

            Assert.AreEqual(expected, target.CurrentPageName);
        }

        #endregion GetCurrentPageNameTest

        #region GetCurrentLanguageTest

        /// <summary>
        ///A test for GetCurrentLanguage
        ///</summary>
        [TestMethod, TestCategory("CurrentLanguage")]
        public void GetCurrentLanguage01Test()
        {
            const string requestUrl = "http://localhost/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "nl";

            Assert.AreEqual(expected, target.CurrentLanguage);
        }

        /// <summary>
        ///A test for GetCurrentLanguage
        ///</summary>
        [TestMethod, TestCategory("CurrentLanguage")]
        public void GetCurrentLanguage02Test()
        {
            const string requestUrl = "http://www.konfidence.nl/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "nl";

            Assert.AreEqual(expected, target.CurrentLanguage);
        }

        /// <summary>
        ///A test for GetCurrentLanguage
        ///</summary>
        [TestMethod, TestCategory("CurrentLanguage")]
        public void GetCurrentLanguage03Test()
        {
            const string requestUrl = "http://www.konfidence.be/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "nl";

            Assert.AreEqual(expected, target.CurrentLanguage);
        }

        /// <summary>
        ///A test for GetCurrentLanguage
        ///</summary>
        [TestMethod, TestCategory("CurrentLanguage")]
        public void GetCurrentLanguage04Test()
        {
            const string requestUrl = "http://www.konfidence.eu/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "uk";

            Assert.AreEqual(expected, target.CurrentLanguage);
        }

        /// <summary>
        ///A test for GetCurrentLanguage
        ///</summary>
        [TestMethod, TestCategory("CurrentLanguage")]
        public void GetCurrentLanguage05Test()
        {
            const string requestUrl = "http://www.konfidence.co.uk/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "uk";

            Assert.AreEqual(expected, target.CurrentLanguage);
        }

        /// <summary>
        ///A test for GetCurrentLanguage
        ///</summary>
        [TestMethod, TestCategory("CurrentLanguage")]
        public void GetCurrentLanguage06Test()
        {
            const string requestUrl = "http://www.konfidence.fr/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "fr";

            Assert.AreEqual(expected, target.CurrentLanguage);
        }

        #endregion GetCurrentLanguageTest

        #region GetCurrentDomainExtensionTest

        /// <summary>
        ///A test for GetCurrentDomainExtension
        ///</summary>
        [TestMethod, TestCategory("CurrentDomainExtension")]
        public void GetCurrentDomainExtension01Test()
        {
            const string requestUrl = "http://localhost/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty); // TODO: Initialize to an appropriate value

            const string expected = "nl";
            
            Assert.AreEqual(expected, target.CurrentDomainExtension);
        }

        /// <summary>
        ///A test for GetCurrentDomainExtension
        ///</summary>
        [TestMethod, TestCategory("CurrentDomainExtension")]
        public void GetCurrentDomainExtension02Test()
        {
            const string requestUrl = "http://www.konfidence.nl/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty); // TODO: Initialize to an appropriate value

            const string expected = "nl";

            Assert.AreEqual(expected, target.CurrentDomainExtension);
        }

        /// <summary>
        ///A test for GetCurrentDomainExtension
        ///</summary>
        [TestMethod, TestCategory("CurrentDomainExtension")]
        public void GetCurrentDomainExtension03Test()
        {
            const string requestUrl = "http://www.konfidence.eu/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty); // TODO: Initialize to an appropriate value

            const string expected = "eu";

            Assert.AreEqual(expected, target.CurrentDomainExtension);
        }

        /// <summary>
        ///A test for GetCurrentDomainExtension
        ///</summary>
        [TestMethod, TestCategory("CurrentDomainExtension")]
        public void GetCurrentDomainExtension04Test()
        {
            const string requestUrl = "http://www.konfidence.co.uk/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty); // TODO: Initialize to an appropriate value

            const string expected = "co.uk";

            Assert.AreEqual(expected, target.CurrentDomainExtension);
        }

        /// <summary>
        ///A test for GetCurrentDomainExtension
        ///</summary>
        [TestMethod, TestCategory("CurrentDomainExtension")]
        public void GetCurrentDomainExtension05Test()
        {
            const string requestUrl = "http://www.konfidence.gov.uk/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty); // TODO: Initialize to an appropriate value

            const string expected = "gov.uk";

            Assert.AreEqual(expected, target.CurrentDomainExtension);
        }

        #endregion GetCurrentDomainExtensionTest

        #region GetCurrentDnsNameTest

        /// <summary>
        ///A test for GetCurrentDnsName
        ///</summary>
        [TestMethod, TestCategory("CurrentDnsName")]
        public void GetCurrentDnsName01Test()
        {
            const string requestUrl = "http://localhost/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty); // TODO: Initialize to an appropriate value

            const string expected = "www.konfidence.nl";

            Assert.AreEqual(expected, target.CurrentDnsName);
        }

        /// <summary>
        ///A test for GetCurrentDnsName
        ///</summary>
        [TestMethod, TestCategory("CurrentDnsName")]
        public void GetCurrentDnsName02Test()
        {
            const string requestUrl = "http://www.konfidence.nl/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty); // TODO: Initialize to an appropriate value

            const string expected = "www.konfidence.nl";

            Assert.AreEqual(expected, target.CurrentDnsName);
        }

        /// <summary>
        ///A test for GetCurrentDnsName
        ///</summary>
        [TestMethod, TestCategory("CurrentDnsName")]
        public void GetCurrentDnsName03Test()
        {
            const string requestUrl = "http://www.konfidence.co.uk/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty); // TODO: Initialize to an appropriate value

            const string expected = "www.konfidence.co.uk";

            Assert.AreEqual(expected, target.CurrentDnsName);
        }

        #endregion GetCurrentDnsNameTest
    }
}
