using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FluentAssertions;
using Konfidence.BaseDatabaseClasses.Objects;
using Konfidence.DataBaseInterface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseDatabaseClasses.Tests
{
    [TestClass]
    public class DbParameterObjectTests
    {
        [TestMethod]
        public void When_GuidValue_Added_To_ListOfIDbParameterObjects_Should_Contain_Guid_DBParameterObject()
        {
            // arrange
            var dbParameterObjects = new List<IDbParameterObject>();
            var testValue = Guid.NewGuid();

            // act
            dbParameterObjects.SetParameter("TestField", testValue);

            var field = dbParameterObjects.First();

            // assert
            dbParameterObjects.Should().HaveCount(1);

            field.Should().NotBeNull();
            field.DbType.Should().Be(DbType.Guid);
            field.Value.Should().Be(testValue);
        }

        [TestMethod]
        public void When_EmptyGuidValue_Added_To_ListOfIDbParameterObjects_Should_Contain_null_DBParameterObject()
        {
            // arrange
            var dbParameterObjects = new List<IDbParameterObject>();
            var testValue = Guid.Empty;

            // act
            dbParameterObjects.SetParameter("TestField", testValue);

            var field = dbParameterObjects.First();

            // assert
            dbParameterObjects.Should().HaveCount(1);

            field.Should().NotBeNull();
            field.DbType.Should().Be(DbType.Guid);
            field.Value.Should().Be(null);
        }

        [TestMethod]
        public void When_IntValue_Added_To_ListOfIDbParameterObjects_Should_Contain_int_DBParameterObject()
        {
            // arrange
            var dbParameterObjects = new List<IDbParameterObject>();
            const int testValue = 1234;

            // act
            dbParameterObjects.SetParameter("TestField", testValue);

            var field = dbParameterObjects.First();

            // assert
            dbParameterObjects.Should().HaveCount(1);

            field.Should().NotBeNull();
            field.DbType.Should().Be(DbType.Int32);
            field.Value.Should().Be(testValue);
        }

        [TestMethod]
        public void When_LongValue_Added_To_ListOfIDbParameterObjects_Should_Contain_long_DBParameterObject()
        {
            // arrange
            var dbParameterObjects = new List<IDbParameterObject>();
            const long testValue = 1234;

            // act
            dbParameterObjects.SetParameter("TestField", testValue);

            var field = dbParameterObjects.First();

            // assert
            dbParameterObjects.Should().HaveCount(1);

            field.Should().NotBeNull();
            field.DbType.Should().Be(DbType.Int64);
            field.Value.Should().Be(testValue);
        }

        [TestMethod]
        public void When_ShortValue_Added_To_ListOfIDbParameterObjects_Should_Contain_short_DBParameterObject()
        {
            // arrange
            var dbParameterObjects = new List<IDbParameterObject>();
            const short testValue = 1234;

            // act
            dbParameterObjects.SetParameter("TestField", testValue);

            var field = dbParameterObjects.First();

            // assert
            dbParameterObjects.Should().HaveCount(1);

            field.Should().NotBeNull();
            field.DbType.Should().Be(DbType.Int16);
            field.Value.Should().Be(testValue);
        }

        [TestMethod]
        public void When_DateTimeValue_Added_To_ListOfIDbParameterObjects_Should_Contain_datetime_DBParameterObject()
        {
            // arrange
            var dbParameterObjects = new List<IDbParameterObject>();
            var testValue = DateTime.Now;

            // act
            dbParameterObjects.SetParameter("TestField", testValue);

            var field = dbParameterObjects.First();

            // assert
            dbParameterObjects.Should().HaveCount(1);

            field.Should().NotBeNull();
            field.DbType.Should().Be(DbType.DateTime);
            field.Value.Should().Be(testValue);
        }

        [TestMethod]
        public void When_DateTimeMinValue_Added_To_ListOfIDbParameterObjects_Should_Contain_null_DBParameterObject()
        {
            // arrange
            var dbParameterObjects = new List<IDbParameterObject>();
            var testValue = DateTime.MinValue;

            // act
            dbParameterObjects.SetParameter("TestField", testValue);

            var field = dbParameterObjects.First();

            // assert
            dbParameterObjects.Should().HaveCount(1);

            field.Should().NotBeNull();
            field.DbType.Should().Be(DbType.DateTime);
            field.Value.Should().Be(null);
        }

        [TestMethod]
        public void When_TimeSpanValue_Added_To_ListOfIDbParameterObjects_Should_Contain_time_DBParameterObject()
        {
            // arrange
            var dbParameterObjects = new List<IDbParameterObject>();
            var testValue = DateTime.Today - DateTime.Today.AddHours(-2).AddSeconds(-22);
            var timeValue = DateTime.Today.AddHours(2).AddSeconds(22);

            // act
            dbParameterObjects.SetParameter("TestField", testValue);

            var field = dbParameterObjects.First();

            // assert
            dbParameterObjects.Should().HaveCount(1);

            field.Should().NotBeNull();
            field.DbType.Should().Be(DbType.Time);
            field.Value.Should().Be(timeValue);
        }

        [TestMethod]
        public void When_TimeSpanMinValue_Added_To_ListOfIDbParameterObjects_Should_Contain_null_DBParameterObject()
        {
            // arrange
            var dbParameterObjects = new List<IDbParameterObject>();
            var testValue = TimeSpan.MinValue;

            // act
            dbParameterObjects.SetParameter("TestField", testValue);

            var field = dbParameterObjects.First();

            // assert
            dbParameterObjects.Should().HaveCount(1);

            field.Should().NotBeNull();
            field.DbType.Should().Be(DbType.Time);
            field.Value.Should().Be(null);
        }


        [TestMethod]
        public void When_BoolValue_Added_To_ListOfIDbParameterObjects_Should_Contain_bool_DBParameterObject()
        {
            // arrange
            var dbParameterObjects = new List<IDbParameterObject>();
            const bool testValue = true;

            // act
            dbParameterObjects.SetParameter("TestField", testValue);

            var field = dbParameterObjects.First();

            // assert
            dbParameterObjects.Should().HaveCount(1);

            field.Should().NotBeNull();
            field.DbType.Should().Be(DbType.Boolean);
            field.Value.Should().Be(testValue);
        }
    }
}
