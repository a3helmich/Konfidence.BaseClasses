using System.Diagnostics.CodeAnalysis;
using DbMenuClasses;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseDatabaseClasses.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class BaseDataItemTest
    {
        [TestMethod]
        public void BaseDataItemWhenInstatiatedShouldSelectClient()
        {
            //var x = "my string";

            //var dataItem = new DataItem();
        }

        [TestMethod]
        public void TestIntDataItemShouldResturnShortAndLong()
        {
            var testIntDataItemList = Bl.TestIntDataItemList.GetList();


            var dataItem = testIntDataItemList[0];
            dataItem.testInt.Should().BeGreaterThan(1);
        }
    }
}
