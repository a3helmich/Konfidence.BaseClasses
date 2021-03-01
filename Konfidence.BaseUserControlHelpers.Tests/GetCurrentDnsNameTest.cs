using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseUserControlHelpers.Tests
{
    /// <summary>
    ///This is a test class for BasePageHelperTest and is intended
    ///to contain all BasePageHelperTest Unit Tests
    ///</summary>
    [TestClass, TestCategory("CurrentDnsName")]
    public class GetCurrentDnsNameTest
    {
        /// <summary>
        ///A test for GetCurrentDnsName
        ///</summary>
        [TestMethod]
        public void GetCurrentDnsName01Test()
        {
            const string requestUrl = "http://localhost/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty); // TODO: Initialize to an appropriate value

            const string expected = "www.konfidence.nl";

            target.CurrentDnsName.Should().Be(expected);
        }

        /// <summary>
        ///A test for GetCurrentDnsName
        ///</summary>
        [TestMethod]
        public void GetCurrentDnsName02Test()
        {
            const string requestUrl = "http://www.konfidence.nl/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty); // TODO: Initialize to an appropriate value

            const string expected = "www.konfidence.nl";

            target.CurrentDnsName.Should().Be(expected);
        }

        /// <summary>
        ///A test for GetCurrentDnsName
        ///</summary>
        [TestMethod]
        public void GetCurrentDnsName03Test()
        {
            const string requestUrl = "http://www.konfidence.co.uk/sitemap/sitemap.aspx";

            var target = new BasePageHelper(requestUrl, string.Empty); // TODO: Initialize to an appropriate value

            const string expected = "www.konfidence.co.uk";

            target.CurrentDnsName.Should().Be(expected);
        }
    }
}
