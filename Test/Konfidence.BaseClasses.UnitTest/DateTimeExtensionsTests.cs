using System;
using FluentAssertions;
using Konfidence.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseClasses.UnitTest;

[TestClass]
public class DateTimeExtensionsTests
{
    private static readonly TimeZoneInfo _utc = TimeZoneInfo.Utc;

    [TestMethod]
    public void ToDateTimeOffset_StringWithExactDateFormat_ReturnsDateAtMidnight()
    {
        // Arrange
        string value = "2026-03-15";

        // Act
        DateTimeOffset result = value.ToDateTimeOffset(_utc);

        // Assert
        result.Should().Be(new DateTimeOffset(2026, 3, 15, 0, 0, 0, TimeSpan.Zero));
    }

    [TestMethod]
    public void ToDateTimeOffset_StringWithGeneralDateFormat_ReturnsParsedDate()
    {
        // Arrange
        string value = "2026-03-15 14:30:00";

        // Act
        DateTimeOffset result = value.ToDateTimeOffset(_utc);

        // Assert
        result.Should().Be(new DateTimeOffset(2026, 3, 15, 14, 30, 0, TimeSpan.Zero));
    }

    [TestMethod]
    public void ToDateTimeOffset_UnparsableString_ReturnsMinValue()
    {
        // Arrange
        string value = "not-a-date";

        // Act
        DateTimeOffset result = value.ToDateTimeOffset(_utc);

        // Assert
        result.Should().Be(DateTimeOffset.MinValue);
    }

    [TestMethod]
    public void ToDateTimeOffset_DateOnly_ReturnsMidnightOffset()
    {
        // Arrange
        DateOnly date = new(2026, 3, 15);

        // Act
        DateTimeOffset result = date.ToDateTimeOffset(_utc);

        // Assert
        result.Should().Be(new DateTimeOffset(2026, 3, 15, 0, 0, 0, TimeSpan.Zero));
    }

    [TestMethod]
    public void ToDateTimeOffset_DateTime_ReturnsOffsetWithGivenTimeZone()
    {
        // Arrange
        DateTime dateTime = new(2026, 3, 15, 14, 30, 0);

        // Act
        DateTimeOffset result = dateTime.ToDateTimeOffset(_utc);

        // Assert
        result.Should().Be(new DateTimeOffset(2026, 3, 15, 14, 30, 0, TimeSpan.Zero));
    }

    [TestMethod]
    public void ToDateTimeOffset_UnixMilliseconds_ReturnsConvertedOffset()
    {
        // Arrange
        DateTimeOffset expected = new(2026, 3, 15, 0, 0, 0, TimeSpan.Zero);
        long unixMilliseconds = expected.ToUnixTimeMilliseconds();

        // Act
        DateTimeOffset result = unixMilliseconds.ToDateTimeOffset(_utc);

        // Assert
        result.Should().Be(expected);
    }

    [TestMethod]
    public void ToDateOnly_FromDateTimeOffset_ReturnsDateComponent()
    {
        // Arrange
        DateTimeOffset dateTimeOffset = new(2026, 3, 15, 14, 30, 0, TimeSpan.Zero);

        // Act
        DateOnly result = dateTimeOffset.ToDateOnly();

        // Assert
        result.Should().Be(new DateOnly(2026, 3, 15));
    }

    [TestMethod]
    public void ToDateOnly_FromDateTime_ReturnsDateComponent()
    {
        // Arrange
        DateTime dateTime = new(2026, 3, 15, 14, 30, 0);

        // Act
        DateOnly result = dateTime.ToDateOnly();

        // Assert
        result.Should().Be(new DateOnly(2026, 3, 15));
    }

    [TestMethod]
    public void ToTimeOnly_FromDateTimeOffset_ReturnsTimeComponent()
    {
        // Arrange
        DateTimeOffset dateTimeOffset = new(2026, 3, 15, 14, 30, 0, TimeSpan.Zero);

        // Act
        TimeOnly result = dateTimeOffset.ToTimeOnly();

        // Assert
        result.Should().Be(new TimeOnly(14, 30, 0));
    }

    [TestMethod]
    public void ToTimeOnly_FromDateTime_ReturnsTimeComponent()
    {
        // Arrange
        DateTime dateTime = new(2026, 3, 15, 14, 30, 0);

        // Act
        TimeOnly result = dateTime.ToTimeOnly();

        // Assert
        result.Should().Be(new TimeOnly(14, 30, 0));
    }

    [TestMethod]
    public void ToDateTime_FromDateOnly_ReturnsMidnight()
    {
        // Arrange
        DateOnly date = new(2026, 3, 15);

        // Act
        DateTime result = date.ToDateTime();

        // Assert
        result.Should().Be(new DateTime(2026, 3, 15, 0, 0, 0));
    }

    [TestMethod]
    public void ToFirstDayInMonth_FromDateTimeOffset_ReturnsFirstOfMonth()
    {
        // Arrange
        DateTimeOffset date = new(2026, 3, 15, 0, 0, 0, TimeSpan.Zero);

        // Act
        DateOnly result = date.ToFirstDayInMonth();

        // Assert
        result.Should().Be(new DateOnly(2026, 3, 1));
    }

    [TestMethod]
    public void ToFirstDayInMonth_FromDateOnly_ReturnsFirstOfMonth()
    {
        // Arrange
        DateOnly date = new(2026, 3, 15);

        // Act
        DateOnly result = date.ToFirstDayInMonth();

        // Assert
        result.Should().Be(new DateOnly(2026, 3, 1));
    }

    [TestMethod]
    public void ToLastDayInMonth_FromDateOnlyInPastMonth_ReturnsLastCalendarDay()
    {
        // Arrange
        DateOnly date = new(2020, 2, 10);

        // Act
        DateOnly result = date.ToLastDayInMonth();

        // Assert
        result.Should().Be(new DateOnly(2020, 2, 29));
    }

    [TestMethod]
    public void ToLastDayInMonth_FromDateTimeOffsetInPastMonth_ReturnsLastCalendarDay()
    {
        // Arrange
        DateTimeOffset date = new(2020, 2, 10, 0, 0, 0, TimeSpan.Zero);

        // Act
        DateOnly result = date.ToLastDayInMonth();

        // Assert
        result.Should().Be(new DateOnly(2020, 2, 29));
    }

    [TestMethod]
    public void ToLastDayInMonth_FromDateOnlyInCurrentMonth_ReturnsToday()
    {
        // Arrange
        DateOnly date = DateOnly.FromDateTime(DateTime.Today);

        // Act
        DateOnly result = date.ToLastDayInMonth();

        // Assert
        result.Should().Be(DateOnly.FromDateTime(DateTime.Today));
    }
}
