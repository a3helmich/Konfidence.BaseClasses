using System;
using System.Globalization;
using JetBrains.Annotations;

namespace Konfidence.Base;


[UsedImplicitly]
public static class DateTimeExtensions
{
    [UsedImplicitly]
    public static DateTimeOffset ToDateTimeOffset(
        this string dateTime,
        TimeZoneInfo timeZoneInfo)
    {
        if (DateTime.TryParseExact(dateTime, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.NoCurrentDateDefault, out DateTime resultDateTime))
        {
            return new DateTimeOffset(resultDateTime.Ticks, timeZoneInfo.BaseUtcOffset);
        }
        
        return DateTime.TryParse(dateTime, CultureInfo.CurrentCulture, DateTimeStyles.NoCurrentDateDefault, out resultDateTime) 
            ? new DateTimeOffset(resultDateTime.Ticks, timeZoneInfo.BaseUtcOffset) 
            : DateTimeOffset.MinValue;
    }

    [UsedImplicitly]
    public static DateTimeOffset ToDateTimeOffset(
        this DateOnly date,
        TimeZoneInfo timeZoneInfo)
    {
        return new DateTimeOffset(date.ToDateTime().Ticks, timeZoneInfo.BaseUtcOffset);
    }

    [UsedImplicitly]
    public static DateTimeOffset ToDateTimeOffset(
        this DateTime dateTime,
        TimeZoneInfo timeZoneInfo)
    {
        return new DateTimeOffset(dateTime.Ticks, timeZoneInfo.BaseUtcOffset);
    }

    [UsedImplicitly]
    public static DateTimeOffset ToDateTimeOffset(
        this long unixMilliseconds,
        TimeZoneInfo timeZoneInfo)
    {
        return TimeZoneInfo.ConvertTime(DateTimeOffset.FromUnixTimeMilliseconds(unixMilliseconds), timeZoneInfo);
    }

    [UsedImplicitly]
    public static DateOnly ToDateOnly(this DateTimeOffset dateTimeOffset)
    {
        return DateOnly.FromDateTime(dateTimeOffset.DateTime);
    }

    [UsedImplicitly]
    public static DateOnly ToDateOnly(this DateTime dateTime)
    {
        return DateOnly.FromDateTime(dateTime);
    }

    [UsedImplicitly]
    public static TimeOnly ToTimeOnly(this DateTimeOffset dateTimeOffset)
    {
        return TimeOnly.FromDateTime(dateTimeOffset.DateTime);
    }

    [UsedImplicitly]
    public static TimeOnly ToTimeOnly(this DateTime dateTime)
    {
        return TimeOnly.FromDateTime(dateTime);
    }

    [UsedImplicitly]
    public static DateTime ToDateTime(this DateOnly date)
    {
        return date.ToDateTime(TimeOnly.MinValue);
    }

    [UsedImplicitly]
    public static DateOnly ToFirstDayInMonth(this DateTimeOffset date)
    {
        return new DateOnly(date.Year, date.Month, 1);
    }

    [UsedImplicitly]
    public static DateOnly ToFirstDayInMonth(this DateOnly date)
    {
        return new DateOnly(date.Year, date.Month, 1);
    }

    [UsedImplicitly]
    public static DateOnly ToLastDayInMonth(this DateTimeOffset date)
    {
        return date.InCurrentMonth()
            ? DateOnly.FromDateTime(DateTime.Today)
            : date.ToFirstDayInMonth().AddMonths(1).AddDays(-1);
    }

    [UsedImplicitly]
    public static DateOnly ToLastDayInMonth(this DateOnly date)
    {
        return date.InCurrentMonth()
            ? DateOnly.FromDateTime(DateTime.Today)
            : date.ToFirstDayInMonth().AddMonths(1).AddDays(-1);
    }

    private static bool InCurrentMonth(this DateTimeOffset date)
    {
        return date.ToDateOnly().InCurrentMonth();
    }

    private static bool InCurrentMonth(this DateOnly date)
    {
        DateTime now = DateTime.Now;

        return now.Year == date.Year && now.Month == date.Month;
    }
}