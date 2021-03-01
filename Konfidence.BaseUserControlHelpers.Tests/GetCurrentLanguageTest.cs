using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseUserControlHelpers.Tests
{
    /// <summary>
    ///This is a test class for BasePageHelperTest and is intended
    ///to contain all BasePageHelperTest Unit Tests
    ///</summary>
    [TestClass, TestCategory("CurrentLanguage")]
    public class GetCurrentLanguageTest
    {
        /// <summary>
        ///A test for GetCurrentLanguage
        ///</summary>
        [TestMethod]
        public void GetCurrentLanguage01Test()
        {
            const string requestUrl = "http://localhost/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "nl";

            target.CurrentLanguage.Should().Be(expected);
        }

        /// <summary>
        ///A test for GetCurrentLanguage
        ///</summary>
        [TestMethod]
        public void GetCurrentLanguage02Test()
        {
            const string requestUrl = "http://www.konfidence.nl/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "nl";

            target.CurrentLanguage.Should().Be(expected);
        }

        /// <summary>
        ///A test for GetCurrentLanguage
        ///</summary>
        [TestMethod]
        public void GetCurrentLanguage03Test()
        {
            const string requestUrl = "http://www.konfidence.be/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "nl";

            target.CurrentLanguage.Should().Be(expected);
        }

        /// <summary>
        ///A test for GetCurrentLanguage
        ///</summary>
        [TestMethod]
        public void GetCurrentLanguage04Test()
        {
            const string requestUrl = "http://www.konfidence.eu/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "uk";

            target.CurrentLanguage.Should().Be(expected);
        }

        /// <summary>
        ///A test for GetCurrentLanguage
        ///</summary>
        [TestMethod]
        public void GetCurrentLanguage05Test()
        {
            const string requestUrl = "http://www.konfidence.co.uk/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "uk";

            target.CurrentLanguage.Should().Be(expected);
        }

        /// <summary>
        ///A test for GetCurrentLanguage
        ///</summary>
        [TestMethod]
        public void GetCurrentLanguage06Test()
        {
            const string requestUrl = "http://www.konfidence.fr/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "fr";

            target.CurrentLanguage.Should().Be(expected);
        }
    }
}
