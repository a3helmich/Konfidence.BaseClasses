using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseUserControlHelpers.Tests
{
    /// <summary>
    ///This is a test class for BasePageHelperTest and is intended
    ///to contain all BasePageHelperTest Unit Tests
    ///</summary>
    [TestClass, TestCategory("CurrentPagePath")]
    public class CurrentPagePathTest
    {
        /// <summary>
        ///A test for GetCurrentPagePath
        ///</summary>
        [TestMethod]
        public void GetCurrentPagePath01Test()
        {
            const string requestUrl = "http://localhost/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "/sitemap/sitemap.aspx";

            target.CurrentPagePath.Should().Be(expected);
        }

        /// <summary>
        ///A test for GetCurrentPagePath
        ///</summary>
        [TestMethod]
        public void GetCurrentPagePath02Test()
        {
            const string requestUrl = "http://www.konfidence.nl/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "/sitemap/sitemap.aspx";

            target.CurrentPagePath.Should().Be(expected);
        }

        /// <summary>
        ///A test for GetCurrentPagePath
        ///</summary>
        [TestMethod]
        public void GetCurrentPagePath03Test()
        {
            const string requestUrl = "http://www.konfidence.nl";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "/";

            target.CurrentPagePath.Should().Be(expected);
        }

        /// <summary>
        ///A test for GetCurrentPagePath
        ///</summary>
        [TestMethod]
        public void GetCurrentPagePath04Test()
        {
            const string requestUrl = "http://www.konfidence.nl/sitemap/h/h/h/h/h/h/h/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "/sitemap/h/h/h/h/h/h/h/sitemap.aspx";

            target.CurrentPagePath.Should().Be(expected);
        }
    }
}
