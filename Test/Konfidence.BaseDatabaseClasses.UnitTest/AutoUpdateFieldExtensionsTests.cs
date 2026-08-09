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
    public void GetAutoUpdateField_Guid_ReturnsStoredValue()
    {
        // Arrange
        TestDataItem dataItem = new();
        Guid storedValue = Guid.NewGuid();
        dataItem.RegisterAutoUpdateField("Field", DbType.Guid, storedValue);
        Guid fieldValue = Guid.Empty;

        // Act
        dataItem.GetAutoUpdateField("Field", ref fieldValue);

        // Assert
        fieldValue.Should().Be(storedValue);
    }

    [TestMethod]
    public void GetAutoUpdateField_DateTime_ReturnsStoredValue()
    {
        // Arrange
        TestDataItem dataItem = new();
        DateTime storedValue = new(2026, 8, 8, 13, 45, 0, DateTimeKind.Utc);
        dataItem.RegisterAutoUpdateField("Field", DbType.DateTime, storedValue);
        DateTime fieldValue = DateTime.MinValue;

        // Act
        dataItem.GetAutoUpdateField("Field", ref fieldValue);

        // Assert
        fieldValue.Should().Be(storedValue);
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

    // Each overload carries its own "?? fieldValue" fallback, so the int test above only proves
    // that one branch - the remaining nine keep-existing-value paths were never exercised.

    [TestMethod]
    public void GetAutoUpdateField_ByteWithUnregisteredField_LeavesValueUnchanged()
    {
        // Arrange
        TestDataItem dataItem = new();
        byte fieldValue = 42;

        // Act
        dataItem.GetAutoUpdateField("Unregistered", ref fieldValue);

        // Assert
        fieldValue.Should().Be(42);
    }

    [TestMethod]
    public void GetAutoUpdateField_ShortWithUnregisteredField_LeavesValueUnchanged()
    {
        // Arrange
        TestDataItem dataItem = new();
        short fieldValue = 42;

        // Act
        dataItem.GetAutoUpdateField("Unregistered", ref fieldValue);

        // Assert
        fieldValue.Should().Be(42);
    }

    [TestMethod]
    public void GetAutoUpdateField_LongWithUnregisteredField_LeavesValueUnchanged()
    {
        // Arrange
        TestDataItem dataItem = new();
        long fieldValue = 42L;

        // Act
        dataItem.GetAutoUpdateField("Unregistered", ref fieldValue);

        // Assert
        fieldValue.Should().Be(42L);
    }

    [TestMethod]
    public void GetAutoUpdateField_GuidWithUnregisteredField_LeavesValueUnchanged()
    {
        // Arrange
        TestDataItem dataItem = new();
        Guid originalValue = Guid.NewGuid();
        Guid fieldValue = originalValue;

        // Act
        dataItem.GetAutoUpdateField("Unregistered", ref fieldValue);

        // Assert
        fieldValue.Should().Be(originalValue);
    }

    [TestMethod]
    public void GetAutoUpdateField_StringWithUnregisteredField_LeavesValueUnchanged()
    {
        // Arrange
        TestDataItem dataItem = new();
        string? fieldValue = "existing";

        // Act
        dataItem.GetAutoUpdateField("Unregistered", ref fieldValue);

        // Assert
        fieldValue.Should().Be("existing");
    }

    [TestMethod]
    public void GetAutoUpdateField_BoolWithUnregisteredField_LeavesValueUnchanged()
    {
        // Arrange
        TestDataItem dataItem = new();
        bool fieldValue = true;

        // Act
        dataItem.GetAutoUpdateField("Unregistered", ref fieldValue);

        // Assert
        fieldValue.Should().BeTrue();
    }

    [TestMethod]
    public void GetAutoUpdateField_DateTimeWithUnregisteredField_LeavesValueUnchanged()
    {
        // Arrange
        TestDataItem dataItem = new();
        DateTime originalValue = new(2026, 8, 8, 13, 45, 0, DateTimeKind.Utc);
        DateTime fieldValue = originalValue;

        // Act
        dataItem.GetAutoUpdateField("Unregistered", ref fieldValue);

        // Assert
        fieldValue.Should().Be(originalValue);
    }

    [TestMethod]
    public void GetAutoUpdateField_TimeSpanWithUnregisteredField_LeavesValueUnchanged()
    {
        // Arrange
        TestDataItem dataItem = new();
        TimeSpan fieldValue = TimeSpan.FromMinutes(5);

        // Act
        dataItem.GetAutoUpdateField("Unregistered", ref fieldValue);

        // Assert
        fieldValue.Should().Be(TimeSpan.FromMinutes(5));
    }

    [TestMethod]
    public void GetAutoUpdateField_DecimalWithUnregisteredField_LeavesValueUnchanged()
    {
        // Arrange
        TestDataItem dataItem = new();
        decimal fieldValue = 7.5m;

        // Act
        dataItem.GetAutoUpdateField("Unregistered", ref fieldValue);

        // Assert
        fieldValue.Should().Be(7.5m);
    }

    [TestMethod]
    public void GetAutoUpdateField_StringOverloadWithNonStringStoredValue_SilentlyYieldsNull()
    {
        // Arrange
        // The string overload ends in "as string" rather than a cast like every other overload,
        // so a type mismatch nulls the field instead of throwing InvalidCastException - and it
        // discards the caller's existing value while doing so.
        TestDataItem dataItem = new();
        dataItem.RegisterAutoUpdateField("Field", DbType.String, 7);
        string? fieldValue = "existing";

        // Act
        dataItem.GetAutoUpdateField("Field", ref fieldValue);

        // Assert
        fieldValue.Should().BeNull();
    }

    [TestMethod]
    public void GetAutoUpdateField_NonStringOverloadWithMismatchedStoredValue_Throws()
    {
        // Arrange
        // Contrast with the string overload above: every other overload uses a hard cast, so the
        // same mismatch surfaces as an exception rather than silently losing the value.
        TestDataItem dataItem = new();
        dataItem.RegisterAutoUpdateField("Field", DbType.Int32, "not an int");
        int fieldValue = 42;

        // Act
        Action action = () =>
        {
            int localFieldValue = fieldValue;

            dataItem.GetAutoUpdateField("Field", ref localFieldValue);
        };

        // Assert
        action.Should().Throw<InvalidCastException>();
    }
}
