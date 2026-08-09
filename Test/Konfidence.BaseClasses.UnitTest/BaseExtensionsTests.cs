using System;
using FluentAssertions;
using Konfidence.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseClasses.UnitTest;

[TestClass]
public class BaseExtensionsTests
{
    [TestMethod]
    public void IsAssigned_NullObject_ReturnsFalse()
    {
        // Arrange
        object? value = null;

        // Act
        bool result = value.IsAssigned();

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsAssigned_AssignedObject_ReturnsTrue()
    {
        // Arrange
        object value = new();

        // Act
        bool result = value.IsAssigned();

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsAssigned_ObjectIsWhitespaceString_ReturnsFalse()
    {
        // Arrange
        object value = "   ";

        // Act
        bool result = value.IsAssigned();

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsAssigned_ObjectIsNonEmptyString_ReturnsTrue()
    {
        // Arrange
        object value = "hello";

        // Act
        bool result = value.IsAssigned();

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsAssigned_ObjectIsEmptyString_ReturnsFalse()
    {
        // Arrange
        object value = string.Empty;

        // Act
        bool result = value.IsAssigned();

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsAssigned_BoxedDefaultInt_ReturnsTrue()
    {
        // Arrange
        object value = 0;

        // Act
        bool result = value.IsAssigned();

        // Assert
        // The whitespace check only applies when the boxed value is a string - a boxed 0 is a
        // real, non-null object and must not be treated as "falsy" the way an empty string is.
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsAssigned_MinValueDateOnly_ReturnsFalse()
    {
        // Arrange
        DateOnly value = DateOnly.MinValue;

        // Act
        bool result = value.IsAssigned();

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsAssigned_ValidDateOnly_ReturnsTrue()
    {
        // Arrange
        DateOnly value = new(2026, 1, 1);

        // Act
        bool result = value.IsAssigned();

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsAssigned_MaxValueDateOnly_ReturnsFalse()
    {
        // Arrange
        DateOnly value = DateOnly.MaxValue;

        // Act
        bool result = value.IsAssigned();

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsAssigned_MinValueTimeSpan_ReturnsFalse()
    {
        // Arrange
        TimeSpan value = TimeSpan.MinValue;

        // Act
        bool result = value.IsAssigned();

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsAssigned_ValidTimeSpan_ReturnsTrue()
    {
        // Arrange
        TimeSpan value = TimeSpan.FromMinutes(5);

        // Act
        bool result = value.IsAssigned();

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsAssigned_MaxValueTimeSpan_ReturnsFalse()
    {
        // Arrange
        TimeSpan value = TimeSpan.MaxValue;

        // Act
        bool result = value.IsAssigned();

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsAssigned_MinValueDateTimeOffset_ReturnsFalse()
    {
        // Arrange
        DateTimeOffset value = DateTimeOffset.MinValue;

        // Act
        bool result = value.IsAssigned();

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsAssigned_ValidDateTimeOffset_ReturnsTrue()
    {
        // Arrange
        DateTimeOffset value = DateTimeOffset.UtcNow;

        // Act
        bool result = value.IsAssigned();

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsAssigned_MaxValueDateTimeOffset_ReturnsFalse()
    {
        // Arrange
        DateTimeOffset value = DateTimeOffset.MaxValue;

        // Act
        bool result = value.IsAssigned();

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsAssigned_EmptyGuid_ReturnsFalse()
    {
        // Arrange
        Guid value = Guid.Empty;

        // Act
        bool result = value.IsAssigned();

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsAssigned_ValidGuid_ReturnsTrue()
    {
        // Arrange
        Guid value = Guid.NewGuid();

        // Act
        bool result = value.IsAssigned();

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsAssigned_MinValueDateTime_ReturnsFalse()
    {
        // Arrange
        DateTime value = DateTime.MinValue;

        // Act
        bool result = value.IsAssigned();

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsAssigned_ValidDateTime_ReturnsTrue()
    {
        // Arrange
        DateTime value = DateTime.UtcNow;

        // Act
        bool result = value.IsAssigned();

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsAssigned_MaxValueDateTime_ReturnsFalse()
    {
        // Arrange
        DateTime value = DateTime.MaxValue;

        // Act
        bool result = value.IsAssigned();

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsEof_NullLine_ReturnsTrue()
    {
        // Arrange
        string? line = null;

        // Act
        bool result = line.IsEof();

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsEof_NonNullLine_ReturnsFalse()
    {
        // Arrange
        const string line = "some text";

        // Act
        bool result = line.IsEof();

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsEof_WhitespaceLine_ReturnsFalse()
    {
        // Arrange
        // Unlike IsAssigned(), which treats whitespace-only strings as unassigned, IsEof() only
        // checks for null - a blank line read from a stream is real data, not end-of-stream.
        const string line = "   ";

        // Act
        bool result = line.IsEof();

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsGuid_ValidGuidString_ReturnsTrue()
    {
        // Arrange
        string value = Guid.NewGuid().ToString();

        // Act
        bool result = value.IsGuid();

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsGuid_InvalidGuidString_ReturnsFalse()
    {
        // Arrange
        const string value = "not-a-guid";

        // Act
        bool result = value.IsGuid();

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsGuid_NullString_ReturnsFalse()
    {
        // Arrange
        string? value = null;

        // Act
        bool result = value!.IsGuid();

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsGuid_EmptyString_ReturnsFalse()
    {
        // Arrange
        string value = string.Empty;

        // Act
        bool result = value.IsGuid();

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsGuid_EmptyGuidString_ReturnsTrue()
    {
        // Arrange
        string value = Guid.Empty.ToString();

        // Act
        bool result = value.IsGuid();

        // Assert
        // IsGuid only checks parseability, not whether the value is the empty Guid - that
        // distinction belongs to IsAssigned(Guid), which treats Guid.Empty as unassigned.
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsNumeric_UnassignedString_ReturnsFalse()
    {
        // Arrange
        string value = string.Empty;

        // Act
        bool result = value.IsNumeric();

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsNumeric_UnsignedIntegerString_ReturnsTrue()
    {
        // Arrange
        const string value = "12345";

        // Act
        bool result = value.IsNumeric();

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsNumeric_NegativeDecimalString_ReturnsTrue()
    {
        // Arrange
        const string value = "-123.45";

        // Act
        bool result = value.IsNumeric();

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsNumeric_NonNumericString_ReturnsFalse()
    {
        // Arrange
        const string value = "abc";

        // Act
        bool result = value.IsNumeric();

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsNumeric_PositiveDecimalString_ReturnsTrue()
    {
        // Arrange
        const string value = "123.45";

        // Act
        bool result = value.IsNumeric();

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsNumeric_WhitespaceString_ReturnsFalse()
    {
        // Arrange
        const string value = "   ";

        // Act
        bool result = value.IsNumeric();

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsNumeric_LoneMinusSign_ReturnsFalse()
    {
        // Arrange
        const string value = "-";

        // Act
        bool result = value.IsNumeric();

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsNumeric_ScientificNotationString_ReturnsTrue()
    {
        // Arrange
        const string value = "1.5e10";

        // Act
        bool result = value.IsNumeric();

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void StartOfDayTime_AnyDateTime_ReturnsMidnightUtc()
    {
        // Arrange
        DateTime dateTime = new(2026, 3, 15, 14, 30, 45, DateTimeKind.Utc);

        // Act
        DateTime result = dateTime.StartOfDayTime();

        // Assert
        result.Should().Be(new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc));
    }

    [TestMethod]
    public void EndOfDayTime_AnyDateTime_ReturnsLastSecondOfDayUtc()
    {
        // Arrange
        DateTime dateTime = new(2026, 3, 15, 14, 30, 45, DateTimeKind.Utc);

        // Act
        DateTime result = dateTime.EndOfDayTime();

        // Assert
        result.Should().Be(new DateTime(2026, 3, 15, 23, 59, 59, DateTimeKind.Utc));
    }

    [TestMethod]
    public void StartOfDayTime_LocalKindInput_ReturnsUtcKindResult()
    {
        // Arrange
        DateTime dateTime = new(2026, 3, 15, 14, 30, 45, DateTimeKind.Local);

        // Act
        DateTime result = dateTime.StartOfDayTime();

        // Assert
        // The method hardcodes DateTimeKind.Utc on the result regardless of the input's Kind - it
        // re-labels the date components rather than converting time zones.
        result.Kind.Should().Be(DateTimeKind.Utc);
        result.Should().Be(new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc));
    }

    [TestMethod]
    public void EndOfDayTime_UnspecifiedKindInput_ReturnsUtcKindResult()
    {
        // Arrange
        DateTime dateTime = new(2026, 3, 15, 14, 30, 45, DateTimeKind.Unspecified);

        // Act
        DateTime result = dateTime.EndOfDayTime();

        // Assert
        result.Kind.Should().Be(DateTimeKind.Utc);
        result.Should().Be(new DateTime(2026, 3, 15, 23, 59, 59, DateTimeKind.Utc));
    }
}
