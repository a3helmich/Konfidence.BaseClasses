using System;
using FluentAssertions;
using Konfidence.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseClasses.UnitTest;

[TestClass]
public class DateTimeExtensionsTests
{
    private static readonly TimeZoneInfo _utc = TimeZoneInfo.Utc;

    // A deterministic, DST-free +02:00 zone, so tests don't depend on the host OS's timezone
    // database (unlike TimeZoneInfo.FindSystemTimeZoneById) or on whatever offset the local
    // machine happens to be in.
    private static readonly TimeZoneInfo _plusTwo = TimeZoneInfo.CreateCustomTimeZone("Test+02:00", TimeSpan.FromHours(2), "Test +02:00", "Test +02:00");

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
    public void ToDateTimeOffset_StringWithNonUtcTimeZone_AppliesTimeZoneOffset()
    {
        // Arrange
        string value = "2026-03-15 14:30:00";

        // Act
        DateTimeOffset result = value.ToDateTimeOffset(_plusTwo);

        // Assert
        result.Should().Be(new DateTimeOffset(2026, 3, 15, 14, 30, 0, TimeSpan.FromHours(2)));
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
    public void ToDateTimeOffset_DateOnlyWithNonUtcTimeZone_AppliesTimeZoneOffset()
    {
        // Arrange
        DateOnly date = new(2026, 3, 15);

        // Act
        DateTimeOffset result = date.ToDateTimeOffset(_plusTwo);

        // Assert
        result.Should().Be(new DateTimeOffset(2026, 3, 15, 0, 0, 0, TimeSpan.FromHours(2)));
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
    public void ToDateTimeOffset_DateTimeWithNonUtcTimeZone_AppliesTimeZoneOffset()
    {
        // Arrange
        DateTime dateTime = new(2026, 3, 15, 14, 30, 0);

        // Act
        DateTimeOffset result = dateTime.ToDateTimeOffset(_plusTwo);

        // Assert
        result.Should().Be(new DateTimeOffset(2026, 3, 15, 14, 30, 0, TimeSpan.FromHours(2)));
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
    public void ToDateTimeOffset_UnixMillisecondsWithNonUtcTimeZone_ConvertsToLocalWallClock()
    {
        // Arrange
        DateTimeOffset utcInstant = new(2026, 3, 15, 0, 0, 0, TimeSpan.Zero);
        long unixMilliseconds = utcInstant.ToUnixTimeMilliseconds();

        // Act
        DateTimeOffset result = unixMilliseconds.ToDateTimeOffset(_plusTwo);

        // Assert
        // ConvertTime shifts the wall-clock time to the target zone's local representation of
        // the same instant, rather than just tagging the UTC time with a different offset.
        result.Should().Be(new DateTimeOffset(2026, 3, 15, 2, 0, 0, TimeSpan.FromHours(2)));
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
    public void ToDateOnly_FromDateTimeOffsetWithNonZeroOffset_ReturnsStoredWallClockDate()
    {
        // Arrange
        DateTimeOffset dateTimeOffset = new(2026, 3, 15, 23, 30, 0, TimeSpan.FromHours(5));

        // Act
        DateOnly result = dateTimeOffset.ToDateOnly();

        // Assert
        // .DateTime returns the wall-clock component as stored in the offset, not converted to
        // UTC - 23:30 at +05:00 must still read back as the 15th, not roll over to the 16th.
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
    public void ToTimeOnly_FromDateTimeOffsetWithNonZeroOffset_ReturnsStoredWallClockTime()
    {
        // Arrange
        DateTimeOffset dateTimeOffset = new(2026, 3, 15, 23, 30, 0, TimeSpan.FromHours(5));

        // Act
        TimeOnly result = dateTimeOffset.ToTimeOnly();

        // Assert
        result.Should().Be(new TimeOnly(23, 30, 0));
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
    public void ToLastDayInMonth_FromDateOnlyInNonLeapFebruaryOfPastYear_Returns28()
    {
        // Arrange
        DateOnly date = new(2021, 2, 10);

        // Act
        DateOnly result = date.ToLastDayInMonth();

        // Assert
        result.Should().Be(new DateOnly(2021, 2, 28));
    }

    [TestMethod]
    public void ToLastDayInMonth_FromDateOnlyInDecemberOfPastYear_ReturnsDecember31()
    {
        // Arrange
        DateOnly date = new(2020, 12, 10);

        // Act
        DateOnly result = date.ToLastDayInMonth();

        // Assert
        // AddMonths(1) crosses into January of the following year before AddDays(-1) brings it
        // back to December 31 - a year-boundary edge, not just a within-year month-length lookup.
        result.Should().Be(new DateOnly(2020, 12, 31));
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

    [TestMethod]
    public void ToLastDayInMonth_FromDateTimeOffsetInCurrentMonth_ReturnsToday()
    {
        // Arrange
        // The DateOnly overload above already exercises its own "current month" branch - this
        // covers the DateTimeOffset overload's private InCurrentMonth(), which had no test at all.
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        DateTimeOffset date = new(today.Year, today.Month, today.Day, 0, 0, 0, TimeSpan.Zero);

        // Act
        DateOnly result = date.ToLastDayInMonth();

        // Assert
        result.Should().Be(DateOnly.FromDateTime(DateTime.Today));
    }
}
