using System.Diagnostics.CodeAnalysis;
using DbMenuClasses;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestExtensionMethods;

namespace Konfidence.BaseDatabaseClasses.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class BaseDataItemTest
    {
        [TestMethod]
        public void TestIntDataItemShouldResturnShortAndLong()
        {
            // arrange
            TestExtensions.CopySqlSettingsToActiveConfiguration();

            var testIntDataItemList = Bl.TestIntDataItemList.GetList();

            // act
            testIntDataItemList.Should().HaveCount(1);

            // assert
            testIntDataItemList[0].testInt.Should().BeGreaterThan(1);
        }
    }
}
