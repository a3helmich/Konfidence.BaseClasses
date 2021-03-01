using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseUserControlHelpers.Tests
{
    /// <summary>
    ///This is a test class for BasePageHelperTest and is intended
    ///to contain all BasePageHelperTest Unit Tests
    ///</summary>
    [TestClass, TestCategory("CurrentDomainExtension")]
    public class GetCurrentDomainExtensionTest
    {
        /// <summary>
        ///A test for GetCurrentDomainExtension
        ///</summary>
        [TestMethod]
        public void GetCurrentDomainExtension01Test()
        {
            const string requestUrl = "http://localhost/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty); // TODO: Initialize to an appropriate value

            const string expected = "nl";

            target.CurrentDomainExtension.Should().Be(expected);
        }

        /// <summary>
        ///A test for GetCurrentDomainExtension
        ///</summary>
        [TestMethod]
        public void GetCurrentDomainExtension02Test()
        {
            const string requestUrl = "http://www.konfidence.nl/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty); // TODO: Initialize to an appropriate value

            const string expected = "nl";

            target.CurrentDomainExtension.Should().Be(expected);
        }

        /// <summary>
        ///A test for GetCurrentDomainExtension
        ///</summary>
        [TestMethod]
        public void GetCurrentDomainExtension03Test()
        {
            const string requestUrl = "http://www.konfidence.eu/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty); // TODO: Initialize to an appropriate value

            const string expected = "eu";

            target.CurrentDomainExtension.Should().Be(expected);
        }

        /// <summary>
        ///A test for GetCurrentDomainExtension
        ///</summary>
        [TestMethod]
        public void GetCurrentDomainExtension04Test()
        {
            const string requestUrl = "http://www.konfidence.co.uk/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty); // TODO: Initialize to an appropriate value

            const string expected = "co.uk";

            target.CurrentDomainExtension.Should().Be(expected);
        }

        /// <summary>
        ///A test for GetCurrentDomainExtension
        ///</summary>
        [TestMethod]
        public void GetCurrentDomainExtension05Test()
        {
            const string requestUrl = "http://www.konfidence.gov.uk/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty); // TODO: Initialize to an appropriate value

            const string expected = "gov.uk";

            target.CurrentDomainExtension.Should().Be(expected);
        }
    }
}
