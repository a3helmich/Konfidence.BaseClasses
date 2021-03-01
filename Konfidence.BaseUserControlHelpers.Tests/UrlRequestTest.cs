using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseUserControlHelpers.Tests
{
    /// <summary>
    ///This is a test class for BasePageHelperTest and is intended
    ///to contain all BasePageHelperTest Unit Tests
    ///</summary>
    [TestClass]
    public class UrlRequestTest
    {
        [TestMethod, TestCategory("UrlRequest")]
        public void UrlRequest00Test()
        {
            var urlRequest = string.Empty;

            var helper = new BasePageHelper(urlRequest, string.Empty);

            helper.IsValid.Should().BeFalse("empty url");
        }

        [TestMethod]
        public void UrlRequest01Test()
        {
            const string urlRequest = "http://localhost";

            var helper = new BasePageHelper(urlRequest, string.Empty);

            helper.IsValid.Should().BeTrue(urlRequest);
        }

        [TestMethod]
        public void UrlRequest02Test()
        {
            const string urlRequest = "http://localhost/";

            var helper = new BasePageHelper(urlRequest, string.Empty);

            helper.IsValid.Should().BeTrue(urlRequest);
        }

        [TestMethod]
        public void UrlRequest03Test()
        {
            const string urlRequest = "http://localhost:8080";

            var helper = new BasePageHelper(urlRequest, string.Empty);

            helper.IsValid.Should().BeTrue(urlRequest);
        }

        [TestMethod]
        public void UrlRequest04Test()
        {
            const string urlRequest = "http://localhost:8080/";

            var helper = new BasePageHelper(urlRequest, string.Empty);

            helper.IsValid.Should().BeTrue(urlRequest);
        }

        [TestMethod]
        public void UrlRequest05Test()
        {
            const string urlRequest = "http://localhost/sitemap.aspx";

            var helper = new BasePageHelper(urlRequest, string.Empty);

            helper.IsValid.Should().BeTrue(urlRequest);
        }

        [TestMethod]
        public void UrlRequest06Test()
        {
            const string urlRequest = "http://localhost:8080/sitemap.aspx";

            var helper = new BasePageHelper(urlRequest, string.Empty);

            helper.IsValid.Should().BeTrue(urlRequest);
        }

        [TestMethod]
        public void UrlRequest07Test()
        {
            const string urlRequest = "http://localhost/sitemap/sitemap.aspx";

            var helper = new BasePageHelper(urlRequest, string.Empty);

            helper.IsValid.Should().BeTrue(urlRequest);
        }

        [TestMethod]
        public void UrlRequest08Test()
        {
            const string urlRequest = "http://localhost:8080/sitemap/sitemap.aspx";

            var helper = new BasePageHelper(urlRequest, string.Empty);

            helper.IsValid.Should().BeTrue(urlRequest);
        }

        [TestMethod]
        public void UrlRequest09Test()
        {
            const string urlRequest = "http://localhost/sitemap/h/h/h/h/h/sitemap.aspx";

            var helper = new BasePageHelper(urlRequest, string.Empty);

            helper.IsValid.Should().BeTrue(urlRequest);
        }

        [TestMethod]
        public void UrlRequest10Test()
        {
            const string urlRequest = "http://localhost:8080/sitemap/h/h/h/h/h/sitemap.aspx";

            var helper = new BasePageHelper(urlRequest, string.Empty);

            helper.IsValid.Should().BeTrue(urlRequest);
        }
    }
}
