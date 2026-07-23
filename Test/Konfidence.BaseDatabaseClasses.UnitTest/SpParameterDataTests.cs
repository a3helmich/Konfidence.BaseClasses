using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FluentAssertions;
using Konfidence.BaseData.Sp;
using Konfidence.DatabaseInterface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseDatabaseClasses.UnitTest
{
    [TestClass]
    public class SpParameterObjectTests
    {
        [TestMethod]
        public void When_GuidValue_Added_To_ListOfIDbParameterObjects_Should_Contain_Guid_DBParameterObject()
        {
            // arrange
            List<ISpParameterData> dbParameterData = [];
            Guid testValue = Guid.NewGuid();

            // act
            dbParameterData.SetParameter("TestField", testValue);

            ISpParameterData field = dbParameterData.First();

            // assert
            dbParameterData.Should().HaveCount(1);

            field.Should().NotBeNull();
            field.DbType.Should().Be(DbType.Guid);
            field.Value.Should().Be(testValue);
        }

        [TestMethod]
        public void When_EmptyGuidValue_Added_To_ListOfIDbParameterObjects_Should_Contain_null_DBParameterObject()
        {
            // arrange
            List<ISpParameterData> dbParameterData = [];
            Guid testValue = Guid.Empty;

            // act
            dbParameterData.SetParameter("TestField", testValue);

            ISpParameterData field = dbParameterData.First();

            // assert
            dbParameterData.Should().HaveCount(1);

            field.Should().NotBeNull();
            field.DbType.Should().Be(DbType.Guid);
            field.Value.Should().Be(null);
        }

        [TestMethod]
        public void When_IntValue_Added_To_ListOfIDbParameterObjects_Should_Contain_int_DBParameterObject()
        {
            // arrange
            List<ISpParameterData> dbParameterData = [];
            const int testValue = 1234;

            // act
            dbParameterData.SetParameter("TestField", testValue);

            ISpParameterData field = dbParameterData.First();

            // assert
            dbParameterData.Should().HaveCount(1);

            field.Should().NotBeNull();
            field.DbType.Should().Be(DbType.Int32);
            field.Value.Should().Be(testValue);
        }

        [TestMethod]
        public void When_LongValue_Added_To_ListOfIDbParameterObjects_Should_Contain_long_DBParameterObject()
        {
            // arrange
            List<ISpParameterData> dbParameterData = [];
            const long testValue = 1234;

            // act
            dbParameterData.SetParameter("TestField", testValue);

            ISpParameterData field = dbParameterData.First();

            // assert
            dbParameterData.Should().HaveCount(1);

            field.Should().NotBeNull();
            field.DbType.Should().Be(DbType.Int64);
            field.Value.Should().Be(testValue);
        }

        [TestMethod]
        public void When_ShortValue_Added_To_ListOfIDbParameterObjects_Should_Contain_short_DBParameterObject()
        {
            // arrange
            List<ISpParameterData> dbParameterData = [];
            const short testValue = 1234;

            // act
            dbParameterData.SetParameter("TestField", testValue);

            ISpParameterData field = dbParameterData.First();

            // assert
            dbParameterData.Should().HaveCount(1);

            field.Should().NotBeNull();
            field.DbType.Should().Be(DbType.Int16);
            field.Value.Should().Be(testValue);
        }

        [TestMethod]
        public void When_DateTimeValue_Added_To_ListOfIDbParameterObjects_Should_Contain_datetime_DBParameterObject()
        {
            // arrange
            List<ISpParameterData> dbParameterData = [];
            DateTime testValue = DateTime.Now;

            // act
            dbParameterData.SetParameter("TestField", testValue);

            ISpParameterData field = dbParameterData.First();

            // assert
            dbParameterData.Should().HaveCount(1);

            field.Should().NotBeNull();
            field.DbType.Should().Be(DbType.DateTime);
            field.Value.Should().Be(testValue);
        }

        [TestMethod]
        public void When_DateTimeMinValue_Added_To_ListOfIDbParameterObjects_Should_Contain_null_DBParameterObject()
        {
            // arrange
            List<ISpParameterData> dbParameterData = [];
            DateTime testValue = DateTime.MinValue;

            // act
            dbParameterData.SetParameter("TestField", testValue);

            ISpParameterData field = dbParameterData.First();

            // assert
            dbParameterData.Should().HaveCount(1);

            field.Should().NotBeNull();
            field.DbType.Should().Be(DbType.DateTime);
            field.Value.Should().Be(null);
        }

        [TestMethod]
        public void When_TimeSpanValue_Added_To_ListOfIDbParameterObjects_Should_Contain_time_DBParameterObject()
        {
            // arrange
            List<ISpParameterData> dbParameterData = [];
            TimeSpan testValue = DateTime.Today - DateTime.Today.AddHours(-2).AddSeconds(-22);
            DateTime timeValue = DateTime.Today.AddHours(2).AddSeconds(22);

            // act
            dbParameterData.SetParameter("TestField", testValue);

            ISpParameterData field = dbParameterData.First();

            // assert
            dbParameterData.Should().HaveCount(1);

            field.Should().NotBeNull();
            field.DbType.Should().Be(DbType.Time);
            field.Value.Should().Be(timeValue);
        }

        [TestMethod]
        public void When_TimeSpanMinValue_Added_To_ListOfIDbParameterObjects_Should_Contain_null_DBParameterObject()
        {
            // arrange
            List<ISpParameterData> dbParameterData = [];
            TimeSpan testValue = TimeSpan.MinValue;

            // act
            dbParameterData.SetParameter("TestField", testValue);

            ISpParameterData field = dbParameterData.First();

            // assert
            dbParameterData.Should().HaveCount(1);

            field.Should().NotBeNull();
            field.DbType.Should().Be(DbType.Time);
            field.Value.Should().Be(null);
        }


        [TestMethod]
        public void When_BoolValue_Added_To_ListOfIDbParameterObjects_Should_Contain_bool_DBParameterObject()
        {
            // arrange
            List<ISpParameterData> dbParameterData = [];
            const bool testValue = true;

            // act
            dbParameterData.SetParameter("TestField", testValue);

            ISpParameterData field = dbParameterData.First();

            // assert
            dbParameterData.Should().HaveCount(1);

            field.Should().NotBeNull();
            field.DbType.Should().Be(DbType.Boolean);
            field.Value.Should().Be(testValue);
        }
    }
}
