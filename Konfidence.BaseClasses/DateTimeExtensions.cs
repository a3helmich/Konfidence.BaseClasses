using System;
using System.Globalization;
using JetBrains.Annotations;

namespace Konfidence.Base;


[UsedImplicitly]
public static class DateTimeExtensions
{
    public static DateTimeOffset DateToDateTimeOffset(
        this string dateTime,
        TimeZoneInfo timeZoneInfo)
    {
        return DateTime.TryParseExact(dateTime, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.NoCurrentDateDefault, out DateTime resultDateTime)
            ? new DateTimeOffset(resultDateTime.Ticks, timeZoneInfo.BaseUtcOffset)
            : DateTimeOffset.MinValue;
    }

    public static DateTimeOffset DateTimeToDateTimeOffset(
        this string dateTime,
        TimeZoneInfo timeZoneInfo)
    {
        return DateTime.TryParse(dateTime, CultureInfo.CurrentCulture, DateTimeStyles.NoCurrentDateDefault, out DateTime resultDateTime) 
            ? new DateTimeOffset(resultDateTime.Ticks, timeZoneInfo.BaseUtcOffset) 
            : DateTimeOffset.MinValue;
    }

    public static DateTimeOffset ToDateTimeOffset(
        this long unixMilliseconds,
        TimeZoneInfo timeZoneInfo)
    {
        return TimeZoneInfo.ConvertTime(DateTimeOffset.FromUnixTimeMilliseconds(unixMilliseconds), timeZoneInfo);
    }

    public static DateOnly ToDateOnly(this DateTimeOffset dateTimeOffset)
    {
        return DateOnly.FromDateTime(dateTimeOffset.DateTime);
    }

    public static TimeOnly ToTimeOnly(this DateTimeOffset dateTimeOffset)
    {
        return TimeOnly.FromDateTime(dateTimeOffset.DateTime);
    }

    public static DateTime ToDateTime(this DateOnly date)
    {
        return date.ToDateTime(TimeOnly.MinValue);
    }

    public static DateOnly ToFirstDayInMonth(this DateOnly date)
    {
        return new DateOnly(date.Year, date.Month, 1);
    }

    public static DateOnly ToLastDayInMonth(this DateOnly date)
    {
        return date.InCurrentMonth()
            ? DateOnly.FromDateTime(DateTime.Today)
            : date.ToFirstDayInMonth().AddMonths(1).AddDays(-1);
    }

    private static bool InCurrentMonth(this DateOnly date)
    {
        DateTime now = DateTime.Now;

        return now.Year == date.Year && now.Month == date.Month;
    }
}