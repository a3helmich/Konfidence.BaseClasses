using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Konfidence.Base;

[UsedImplicitly]
public static class BaseExtensions
{
    [ContractAnnotation("assignedObject:null => false")]
#nullable disable
    public static bool IsAssigned([NotNullWhen(true)] this object assignedObject)
    {
#nullable restore
        if (assignedObject is string assignedString)
        {
            return !string.IsNullOrWhiteSpace(assignedString);
        }

        return assignedObject is not null;
    }

    [UsedImplicitly]
    public static bool IsAssigned(this DateOnly assignedDate)
    {
        return assignedDate > DateOnly.MinValue && assignedDate < DateOnly.MaxValue;
    }

    [UsedImplicitly]
    public static bool IsAssigned(this TimeSpan assignedTimeSpan)
    {
        return assignedTimeSpan > TimeSpan.MinValue && assignedTimeSpan < TimeSpan.MaxValue;
    }

    [UsedImplicitly]
    public static bool IsAssigned(this DateTimeOffset assignedDateTimeOffset)
    {
        return assignedDateTimeOffset > DateTimeOffset.MinValue && assignedDateTimeOffset < DateTimeOffset.MaxValue;
    }

    [ContractAnnotation("assignedGuid:null => false")]
    public static bool IsAssigned(this Guid assignedGuid)
    {
        return !Guid.Empty.Equals(assignedGuid);
    }

    [UsedImplicitly]
    [ContractAnnotation("line:null => true")]
#nullable disable
    public static bool IsEof([NotNullWhen(false)] this string line)
    {
#nullable restore
        return line is null;
    }

    [UsedImplicitly]
    [ContractAnnotation("assignedGuid:null => false")]
#nullable disable
    public static bool IsGuid([NotNullWhen(true)] this string assignedGuid)
    {
#nullable restore
        return Guid.TryParse(assignedGuid, out _);
    }

    [UsedImplicitly]
    public static bool IsNumeric(this string numericString)
    {
        if (!numericString.IsAssigned())
        {
            return false;
        }

        if (ulong.TryParse(numericString.TrimStart('-'), out _))
        {
            return true;
        }

        return double.TryParse(numericString, out _) || decimal.TryParse(numericString, out _);
    }

    extension(DateTime dateTime)
    {
        [UsedImplicitly]
        public DateTime StartOfDayTime()
        {
            DateTime afterMidnight = new(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, DateTimeKind.Utc);

            return afterMidnight;
        }

        [UsedImplicitly]
        public DateTime EndOfDayTime()
        {
            DateTime midnight = new(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, DateTimeKind.Utc);

            return midnight;
        }

        [UsedImplicitly]
        public bool IsAssigned()
        {
            return dateTime > DateTime.MinValue && dateTime < DateTime.MaxValue;
        }
    }
}
