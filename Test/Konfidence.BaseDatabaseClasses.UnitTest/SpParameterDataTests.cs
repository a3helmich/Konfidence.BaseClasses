using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FluentAssertions;
using Konfidence.BaseData.Sp;
using Konfidence.DatabaseInterface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseDatabaseClasses.UnitTest;

[TestClass]
public class SpParameterObjectTests
{
    [TestMethod]
    public void SetParameter_WithGuidValue_ContainsGuidParameterObject()
    {
        // Arrange
        List<ISpParameterData> dbParameterData = [];
        Guid testValue = Guid.NewGuid();

        // Act
        dbParameterData.SetParameter("TestField", testValue);

        ISpParameterData field = dbParameterData.First();

        // Assert
        dbParameterData.Should().HaveCount(1);

        field.Should().NotBeNull();
        field.DbType.Should().Be(DbType.Guid);
        field.Value.Should().Be(testValue);
    }

    [TestMethod]
    public void SetParameter_WithEmptyGuidValue_ContainsNullParameterObject()
    {
        // Arrange
        List<ISpParameterData> dbParameterData = [];
        Guid testValue = Guid.Empty;

        // Act
        dbParameterData.SetParameter("TestField", testValue);

        ISpParameterData field = dbParameterData.First();

        // Assert
        dbParameterData.Should().HaveCount(1);

        field.Should().NotBeNull();
        field.DbType.Should().Be(DbType.Guid);
        field.Value.Should().Be(null);
    }

    [TestMethod]
    public void SetParameter_WithIntValue_ContainsIntParameterObject()
    {
        // Arrange
        List<ISpParameterData> dbParameterData = [];
        const int testValue = 1234;

        // Act
        dbParameterData.SetParameter("TestField", testValue);

        ISpParameterData field = dbParameterData.First();

        // Assert
        dbParameterData.Should().HaveCount(1);

        field.Should().NotBeNull();
        field.DbType.Should().Be(DbType.Int32);
        field.Value.Should().Be(testValue);
    }

    [TestMethod]
    public void SetParameter_WithLongValue_ContainsLongParameterObject()
    {
        // Arrange
        List<ISpParameterData> dbParameterData = [];
        const long testValue = 1234;

        // Act
        dbParameterData.SetParameter("TestField", testValue);

        ISpParameterData field = dbParameterData.First();

        // Assert
        dbParameterData.Should().HaveCount(1);

        field.Should().NotBeNull();
        field.DbType.Should().Be(DbType.Int64);
        field.Value.Should().Be(testValue);
    }

    [TestMethod]
    public void SetParameter_WithShortValue_ContainsShortParameterObject()
    {
        // Arrange
        List<ISpParameterData> dbParameterData = [];
        const short testValue = 1234;

        // Act
        dbParameterData.SetParameter("TestField", testValue);

        ISpParameterData field = dbParameterData.First();

        // Assert
        dbParameterData.Should().HaveCount(1);

        field.Should().NotBeNull();
        field.DbType.Should().Be(DbType.Int16);
        field.Value.Should().Be(testValue);
    }

    [TestMethod]
    public void SetParameter_WithDateTimeValue_ContainsDateTimeParameterObject()
    {
        // Arrange
        List<ISpParameterData> dbParameterData = [];
        DateTime testValue = DateTime.Now;

        // Act
        dbParameterData.SetParameter("TestField", testValue);

        ISpParameterData field = dbParameterData.First();

        // Assert
        dbParameterData.Should().HaveCount(1);

        field.Should().NotBeNull();
        field.DbType.Should().Be(DbType.DateTime);
        field.Value.Should().Be(testValue);
    }

    [TestMethod]
    public void SetParameter_WithDateTimeMinValue_ContainsNullParameterObject()
    {
        // Arrange
        List<ISpParameterData> dbParameterData = [];
        DateTime testValue = DateTime.MinValue;

        // Act
        dbParameterData.SetParameter("TestField", testValue);

        ISpParameterData field = dbParameterData.First();

        // Assert
        dbParameterData.Should().HaveCount(1);

        field.Should().NotBeNull();
        field.DbType.Should().Be(DbType.DateTime);
        field.Value.Should().Be(null);
    }

    [TestMethod]
    public void SetParameter_WithTimeSpanValue_ContainsTimeParameterObject()
    {
        // Arrange
        List<ISpParameterData> dbParameterData = [];
        TimeSpan testValue = DateTime.Today - DateTime.Today.AddHours(-2).AddSeconds(-22);
        DateTime timeValue = DateTime.Today.AddHours(2).AddSeconds(22);

        // Act
        dbParameterData.SetParameter("TestField", testValue);

        ISpParameterData field = dbParameterData.First();

        // Assert
        dbParameterData.Should().HaveCount(1);

        field.Should().NotBeNull();
        field.DbType.Should().Be(DbType.Time);
        field.Value.Should().Be(timeValue);
    }

    [TestMethod]
    public void SetParameter_WithTimeSpanMinValue_ContainsNullParameterObject()
    {
        // Arrange
        List<ISpParameterData> dbParameterData = [];
        TimeSpan testValue = TimeSpan.MinValue;

        // Act
        dbParameterData.SetParameter("TestField", testValue);

        ISpParameterData field = dbParameterData.First();

        // Assert
        dbParameterData.Should().HaveCount(1);

        field.Should().NotBeNull();
        field.DbType.Should().Be(DbType.Time);
        field.Value.Should().Be(null);
    }


    [TestMethod]
    public void SetParameter_WithBoolValue_ContainsBoolParameterObject()
    {
        // Arrange
        List<ISpParameterData> dbParameterData = [];
        const bool testValue = true;

        // Act
        dbParameterData.SetParameter("TestField", testValue);

        ISpParameterData field = dbParameterData.First();

        // Assert
        dbParameterData.Should().HaveCount(1);

        field.Should().NotBeNull();
        field.DbType.Should().Be(DbType.Boolean);
        field.Value.Should().Be(testValue);
    }
}
