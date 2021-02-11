﻿using System.Linq;
using DbMenuClasses;
using FluentAssertions;
using Konfidence.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseDatabaseClasses.Integration.Tests
{
    [TestClass]
    public class BaseDataItemTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            SqlTestToolExtensions.CopySqlSettingsToActiveConfiguration();

            var testIntDataItemList = Bl.TestIntDataItem.GetList().Where(x => x.GetId() > 1).ToList();

            if (testIntDataItemList.Any())
            {
                foreach (var intDataItem in testIntDataItemList)
                {
                    intDataItem.Delete();
                }
            }
        }

        [TestMethod]
        public void TestIntDataItemShouldReturnShortAndLong()
        {
            // arrange
            //var testIntDataItemList = Bl.TestIntDataItemList.GetList();
            var testIntDataItemList = Bl.TestIntDataItem.GetList();

            // act

            // assert
            testIntDataItemList.Should().HaveCount(1);
            testIntDataItemList[0].testInt.Should().BeGreaterThan(1);

            testIntDataItemList[0].testTinyInt.Should().Be(10);
            testIntDataItemList[0].testInt.Should().Be(1000);
            testIntDataItemList[0].testBigInt.Should().Be(100);
        }

        [TestMethod]
        public void When_TestIntDataItem_is_Created_when_queried_should_be_returned()
        {
            // arrange
            var testIntDataItem = new Bl.TestIntDataItem();

            // act
            testIntDataItem.testTinyInt = 111;
            testIntDataItem.testInt = 1111;
            testIntDataItem.testBigInt = 11111;

            testIntDataItem.Save();

            var copyTestIntDataItem = new Bl.TestIntDataItem(testIntDataItem.GetId());

            // assert
            copyTestIntDataItem.Id.Should().Be(testIntDataItem.Id);
            copyTestIntDataItem.TestIntId.Should().Be(testIntDataItem.TestIntId);

            copyTestIntDataItem.testTinyInt.Should().Be(111);
            copyTestIntDataItem.testInt.Should().Be(1111);
            copyTestIntDataItem.testBigInt.Should().Be(11111);
        }

        [TestMethod]
        public void When_TestIntDataItem_is_Created_and_updated_when_queried_should_be_returned_update()
        {
            // arrange
            var testIntDataItem = new Bl.TestIntDataItem();

            // act
            testIntDataItem.testTinyInt = 11;
            testIntDataItem.testInt = 1111;
            testIntDataItem.testBigInt = 11111;

            testIntDataItem.Save();

            var copyTestIntDataItem = new Bl.TestIntDataItem(testIntDataItem.Id);

            copyTestIntDataItem.testTinyInt = 222;
            copyTestIntDataItem.testInt = 2222;
            copyTestIntDataItem.testBigInt = 22222;

            copyTestIntDataItem.Save();

            var updateTestIntDataItem = new Bl.TestIntDataItem(testIntDataItem.Id);

            // assert
            updateTestIntDataItem.TestIntId.Should().Be(testIntDataItem.TestIntId);
            updateTestIntDataItem.Id.Should().Be(testIntDataItem.Id);

            updateTestIntDataItem.testTinyInt.Should().Be(222);
            updateTestIntDataItem.testInt.Should().Be(2222);
            updateTestIntDataItem.testBigInt.Should().Be(22222);
        }
    }
}
