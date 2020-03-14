﻿using System.Diagnostics.CodeAnalysis;
using DbMenuClasses;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Konfidence.TestTools;

namespace Konfidence.BaseDatabaseClasses.Tests
{
    [TestClass]
    public class BaseDataItemTest
    {
        [TestMethod]
        public void TestIntDataItemShouldReturnShortAndLong()
        {
            // arrange
            SqlTestToolExtensions.CopySqlSettingsToActiveConfiguration();

            var testIntDataItemList = Bl.TestIntDataItemList.GetList();

            // act
            testIntDataItemList.Should().HaveCount(1);

            // assert
            testIntDataItemList[0].testInt.Should().BeGreaterThan(1);
        }
    }
}
