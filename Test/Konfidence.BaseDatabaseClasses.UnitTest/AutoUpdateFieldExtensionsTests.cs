using System;
using System.Data;
using FluentAssertions;
using Konfidence.BaseData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseDatabaseClasses.UnitTest;

[TestClass]
public class AutoUpdateFieldExtensionsTests
{
    private sealed class TestDataItem : BaseDataItem
    {
        public void RegisterAutoUpdateField(string fieldName, DbType dbType, object? value)
        {
            AddAutoUpdateField(fieldName, dbType);

            AutoUpdateFieldDictionary[fieldName].Value = value;
        }
    }

    [TestMethod]
    public void GetAutoUpdateField_byte_Should_return_stored_value()
    {
        // Arrange
        TestDataItem dataItem = new();
        dataItem.RegisterAutoUpdateField("Field", DbType.Byte, (byte)7);
        byte fieldValue = 0;

        // Act
        dataItem.GetAutoUpdateField("Field", ref fieldValue);

        // Assert
        fieldValue.Should().Be(7);
    }

    [TestMethod]
    public void GetAutoUpdateField_short_Should_return_stored_value()
    {
        // Arrange
        TestDataItem dataItem = new();
        dataItem.RegisterAutoUpdateField("Field", DbType.Int16, (short)7);
        short fieldValue = 0;

        // Act
        dataItem.GetAutoUpdateField("Field", ref fieldValue);

        // Assert
        fieldValue.Should().Be(7);
    }

    [TestMethod]
    public void GetAutoUpdateField_int_Should_return_stored_value()
    {
        // Arrange
        TestDataItem dataItem = new();
        dataItem.RegisterAutoUpdateField("Field", DbType.Int32, 7);
        int fieldValue = 0;

        // Act
        dataItem.GetAutoUpdateField("Field", ref fieldValue);

        // Assert
        fieldValue.Should().Be(7);
    }

    [TestMethod]
    public void GetAutoUpdateField_long_Should_return_stored_value()
    {
        // Arrange
        TestDataItem dataItem = new();
        dataItem.RegisterAutoUpdateField("Field", DbType.Int64, 7L);
        long fieldValue = 0;

        // Act
        dataItem.GetAutoUpdateField("Field", ref fieldValue);

        // Assert
        fieldValue.Should().Be(7L);
    }

    [TestMethod]
    public void GetAutoUpdateField_string_Should_return_stored_value()
    {
        // Arrange
        TestDataItem dataItem = new();
        dataItem.RegisterAutoUpdateField("Field", DbType.String, "value");
        string? fieldValue = null;

        // Act
        dataItem.GetAutoUpdateField("Field", ref fieldValue);

        // Assert
        fieldValue.Should().Be("value");
    }

    [TestMethod]
    public void GetAutoUpdateField_bool_Should_return_stored_value()
    {
        // Arrange
        TestDataItem dataItem = new();
        dataItem.RegisterAutoUpdateField("Field", DbType.Boolean, true);
        bool fieldValue = false;

        // Act
        dataItem.GetAutoUpdateField("Field", ref fieldValue);

        // Assert
        fieldValue.Should().BeTrue();
    }

    [TestMethod]
    public void GetAutoUpdateField_TimeSpan_Should_return_stored_value()
    {
        // Arrange
        TestDataItem dataItem = new();
        TimeSpan storedValue = TimeSpan.FromMinutes(5);
        dataItem.RegisterAutoUpdateField("Field", DbType.Time, storedValue);
        TimeSpan fieldValue = TimeSpan.Zero;

        // Act
        dataItem.GetAutoUpdateField("Field", ref fieldValue);

        // Assert
        fieldValue.Should().Be(storedValue);
    }

    [TestMethod]
    public void GetAutoUpdateField_decimal_Should_return_stored_value()
    {
        // Arrange
        TestDataItem dataItem = new();
        dataItem.RegisterAutoUpdateField("Field", DbType.Decimal, 7.5m);
        decimal fieldValue = 0;

        // Act
        dataItem.GetAutoUpdateField("Field", ref fieldValue);

        // Assert
        fieldValue.Should().Be(7.5m);
    }

    [TestMethod]
    public void GetAutoUpdateField_With_unregistered_field_Should_leave_value_unchanged()
    {
        // Arrange
        TestDataItem dataItem = new();
        int fieldValue = 42;

        // Act
        dataItem.GetAutoUpdateField("Unregistered", ref fieldValue);

        // Assert
        fieldValue.Should().Be(42);
    }
}
