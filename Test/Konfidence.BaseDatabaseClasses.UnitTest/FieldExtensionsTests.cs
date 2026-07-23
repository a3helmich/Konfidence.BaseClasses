using System;
using System.Data;
using System.Linq;
using FluentAssertions;
using Konfidence.BaseData;
using Konfidence.DatabaseInterface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseDatabaseClasses.UnitTest;

[TestClass]
public class FieldExtensionsTests
{
    private sealed class TestDataItem : BaseDataItem;

    [TestMethod]
    public void SetField_short_Should_add_parameter_with_Int16_type_and_value()
    {
        // Arrange
        TestDataItem dataItem = new();

        // Act
        dataItem.SetField("Field", (short)7);

        // Assert
        ISpParameterData parameter = dataItem.GetParameterObjects().Single();

        parameter.DbType.Should().Be(DbType.Int16);
        parameter.Value.Should().Be((short)7);
    }

    [TestMethod]
    public void SetField_bool_Should_add_parameter_with_Boolean_type_and_value()
    {
        // Arrange
        TestDataItem dataItem = new();

        // Act
        dataItem.SetField("Field", true);

        // Assert
        ISpParameterData parameter = dataItem.GetParameterObjects().Single();

        parameter.DbType.Should().Be(DbType.Boolean);
        parameter.Value.Should().Be(true);
    }

    [TestMethod]
    public void SetField_DateTime_Should_add_parameter_with_DateTime_type_and_value()
    {
        // Arrange
        TestDataItem dataItem = new();
        DateTime value = DateTime.Now;

        // Act
        dataItem.SetField("Field", value);

        // Assert
        ISpParameterData parameter = dataItem.GetParameterObjects().Single();

        parameter.DbType.Should().Be(DbType.DateTime);
        parameter.Value.Should().Be(value);
    }

    [TestMethod]
    public void SetField_TimeSpan_Should_add_parameter_with_Time_type_and_value()
    {
        // Arrange
        TestDataItem dataItem = new();
        TimeSpan value = DateTime.Today - DateTime.Today.AddHours(-2).AddSeconds(-22);
        DateTime timeValue = DateTime.Today.AddHours(2).AddSeconds(22);

        // Act
        dataItem.SetField("Field", value);

        // Assert
        ISpParameterData parameter = dataItem.GetParameterObjects().Single();

        parameter.DbType.Should().Be(DbType.Time);
        parameter.Value.Should().Be(timeValue);
    }

    [TestMethod]
    public void SetField_decimal_Should_add_parameter_with_Decimal_type_and_value()
    {
        // Arrange
        TestDataItem dataItem = new();

        // Act
        dataItem.SetField("Field", 7.5m);

        // Assert
        ISpParameterData parameter = dataItem.GetParameterObjects().Single();

        parameter.DbType.Should().Be(DbType.Decimal);
        parameter.Value.Should().Be(7.5m);
    }
}
