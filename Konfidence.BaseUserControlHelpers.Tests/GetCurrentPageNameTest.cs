using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseUserControlHelpers.Tests
{
    /// <summary>
    ///This is a test class for BasePageHelperTest and is intended
    ///to contain all BasePageHelperTest Unit Tests
    ///</summary>
    [TestClass, TestCategory("CurrentPageName")]
    public class GetCurrentPageNameTest
    {
        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod]
        public void GetCurrentPageName01Test()
        {
            const string requestUrl = "http://localhost/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "sitemap.aspx";

            target.CurrentPageName.Should().Be(expected);
        }

        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod]
        public void GetCurrentPageName02Test()
        {
            const string requestUrl = "http://localhost:8080/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "sitemap.aspx";

            target.CurrentPageName.Should().Be(expected);
        }

        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod]
        public void GetCurrentPageName03Test()
        {
            const string requestUrl = "http://localhost/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "sitemap.aspx";

            target.CurrentPageName.Should().Be(expected);
        }

        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod]
        public void GetCurrentPageName04Test()
        {
            const string requestUrl = "http://localhost:8080/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty);

            const string expected = "sitemap.aspx";

            target.CurrentPageName.Should().Be(expected);
        }

        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod]
        public void GetCurrentPageName05Test()
        {
            const string requestUrl = "http://localhost/";

            var target = new BasePageHelper(requestUrl, string.Empty);

            var expected = string.Empty;

            target.CurrentPageName.Should().Be(expected);
        }

        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod]
        public void GetCurrentPageName06Test()
        {
            const string requestUrl = "http://localhost:8080/";

            var target = new BasePageHelper(requestUrl, string.Empty);

            var expected = string.Empty;

            target.CurrentPageName.Should().Be(expected);
        }

        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod]
        public void GetCurrentPageName07Test()
        {
            const string requestUrl = "http://localhost";

            var target = new BasePageHelper(requestUrl, string.Empty);

            var expected = string.Empty;

            target.CurrentPageName.Should().Be(expected);
        }

        /// <summary>
        ///A test for GetCurrentPageName
        ///</summary>
        [TestMethod]
        public void GetCurrentPageName08Test()
        {
            const string requestUrl = "http://localhost:8080";

            var target = new BasePageHelper(requestUrl, string.Empty);

            var expected = string.Empty;

            target.CurrentPageName.Should().Be(expected);
        }
    }
}
