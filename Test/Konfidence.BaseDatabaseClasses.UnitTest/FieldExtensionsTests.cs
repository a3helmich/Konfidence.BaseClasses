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
    public void SetField_Short_AddsParameterWithInt16TypeAndValue()
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
    public void SetField_Int_AddsParameterWithInt32TypeAndValue()
    {
        // Arrange
        TestDataItem dataItem = new();

        // Act
        dataItem.SetField("Field", 7);

        // Assert
        ISpParameterData parameter = dataItem.GetParameterObjects().Single();

        parameter.DbType.Should().Be(DbType.Int32);
        parameter.Value.Should().Be(7);
    }

    [TestMethod]
    public void SetField_Byte_AddsParameterWithByteTypeAndValue()
    {
        // Arrange
        TestDataItem dataItem = new();

        // Act
        dataItem.SetField("Field", (byte)7);

        // Assert
        ISpParameterData parameter = dataItem.GetParameterObjects().Single();

        parameter.DbType.Should().Be(DbType.Byte);
        parameter.Value.Should().Be((byte)7);
    }

    [TestMethod]
    public void SetField_Long_AddsParameterWithInt64TypeAndValue()
    {
        // Arrange
        TestDataItem dataItem = new();

        // Act
        dataItem.SetField("Field", 7L);

        // Assert
        ISpParameterData parameter = dataItem.GetParameterObjects().Single();

        parameter.DbType.Should().Be(DbType.Int64);
        parameter.Value.Should().Be(7L);
    }

    [TestMethod]
    public void SetField_String_AddsParameterWithStringTypeAndValue()
    {
        // Arrange
        TestDataItem dataItem = new();

        // Act
        dataItem.SetField("Field", "value");

        // Assert
        ISpParameterData parameter = dataItem.GetParameterObjects().Single();

        parameter.DbType.Should().Be(DbType.String);
        parameter.Value.Should().Be("value");
    }

    [TestMethod]
    public void SetField_AssignedGuid_AddsParameterWithGuidTypeAndValue()
    {
        // Arrange
        TestDataItem dataItem = new();
        Guid value = Guid.NewGuid();

        // Act
        dataItem.SetField("Field", value);

        // Assert
        ISpParameterData parameter = dataItem.GetParameterObjects().Single();

        parameter.DbType.Should().Be(DbType.Guid);
        parameter.Value.Should().Be(value);
    }

    [TestMethod]
    public void SetField_EmptyGuid_AddsParameterWithNullValue()
    {
        // Arrange
        // Guid.Empty counts as unassigned, so the parameter has to carry a null rather than
        // sending an all-zero Guid to the database.
        TestDataItem dataItem = new();

        // Act
        dataItem.SetField("Field", Guid.Empty);

        // Assert
        ISpParameterData parameter = dataItem.GetParameterObjects().Single();

        parameter.DbType.Should().Be(DbType.Guid);
        parameter.Value.Should().BeNull();
    }

    [TestMethod]
    public void SetField_UnassignedDateTime_AddsParameterWithNullValue()
    {
        // Arrange
        // Same contract as the Guid overload: DateTime.MinValue is unassigned and must become a
        // null parameter instead of year-one being written to the column.
        TestDataItem dataItem = new();

        // Act
        dataItem.SetField("Field", DateTime.MinValue);

        // Assert
        ISpParameterData parameter = dataItem.GetParameterObjects().Single();

        parameter.DbType.Should().Be(DbType.DateTime);
        parameter.Value.Should().BeNull();
    }

    [TestMethod]
    public void SetField_UnassignedTimeSpan_AddsParameterWithNullValue()
    {
        // Arrange
        // IsAssigned(TimeSpan) is a range check (> MinValue && < MaxValue), so only the extremes
        // are unassigned - see the zero-TimeSpan test below for why Zero does not qualify.
        TestDataItem dataItem = new();

        // Act
        dataItem.SetField("Field", TimeSpan.MinValue);

        // Assert
        ISpParameterData parameter = dataItem.GetParameterObjects().Single();

        parameter.DbType.Should().Be(DbType.Time);
        parameter.Value.Should().BeNull();
    }

    [TestMethod]
    public void SetField_ZeroTimeSpan_AddsParameterWithMidnightTodayRatherThanNull()
    {
        // Arrange
        // The three "unassigned becomes null" overloads are not consistent with each other:
        // Guid.Empty and DateTime.MinValue both null out, but TimeSpan.Zero is a legitimate
        // in-range value, so it converts to midnight today like any other time-of-day. A caller
        // treating "default(TimeSpan)" as "no value" would silently write midnight instead.
        TestDataItem dataItem = new();

        // Act
        dataItem.SetField("Field", TimeSpan.Zero);

        // Assert
        ISpParameterData parameter = dataItem.GetParameterObjects().Single();

        parameter.DbType.Should().Be(DbType.Time);
        parameter.Value.Should().Be(DateTime.Today);
    }

    [TestMethod]
    public void SetField_Bool_AddsParameterWithBooleanTypeAndValue()
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
    public void SetField_DateTime_AddsParameterWithDateTimeTypeAndValue()
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
    public void SetField_TimeSpan_AddsParameterWithTimeTypeAndValue()
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
    public void SetField_Decimal_AddsParameterWithDecimalTypeAndValue()
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
