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
    public void GetAutoUpdateField_Byte_ReturnsStoredValue()
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
    public void GetAutoUpdateField_Short_ReturnsStoredValue()
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
    public void GetAutoUpdateField_Int_ReturnsStoredValue()
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
    public void GetAutoUpdateField_Long_ReturnsStoredValue()
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
    public void GetAutoUpdateField_String_ReturnsStoredValue()
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
    public void GetAutoUpdateField_Bool_ReturnsStoredValue()
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
    public void GetAutoUpdateField_TimeSpan_ReturnsStoredValue()
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
    public void GetAutoUpdateField_Decimal_ReturnsStoredValue()
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
    public void GetAutoUpdateField_WithUnregisteredField_LeavesValueUnchanged()
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
