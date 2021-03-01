using System;
using JetBrains.Annotations;

namespace Konfidence.Base
{
    [UsedImplicitly]
    public static class BaseExtensions
    {
        [ContractAnnotation("assignedObject:null => false")]
        public static bool IsAssigned(this object assignedObject)
        {
            if (assignedObject is string assignedString)
            {
                return !string.IsNullOrWhiteSpace(assignedString);
            }

            return assignedObject != null;
        }

        public static bool IsAssigned(this DateTime assignedTime)
        {
            return assignedTime > DateTime.MinValue && assignedTime < DateTime.MaxValue;
        }

        public static bool IsAssigned(this TimeSpan assignedTime)
        {
            return assignedTime > TimeSpan.MinValue && assignedTime < TimeSpan.MaxValue;
        }

        [ContractAnnotation("assignedGuid:null => false")]
        public static bool IsAssigned(this Guid assignedGuid)
        {
            return !Guid.Empty.Equals(assignedGuid);
        }

        [UsedImplicitly]
        [ContractAnnotation("line:null => true")]
        public static bool IsEof(this string line)
        {
            return line == null;
        }

        [UsedImplicitly]
        [ContractAnnotation("assignedGuid:null => false")]
        public static bool IsGuid(this string assignedGuid)
        {
            return Guid.TryParse(assignedGuid, out _);
        }

        [UsedImplicitly]
        public static bool IsNumeric(this string numericString)
        {
            if (!numericString.IsAssigned())
            {
                return false;
            }

            if (long.TryParse(numericString, out _))
            {
                return true;
            }

            if (double.TryParse(numericString, out _))
            {
                return true;
            }

            return decimal.TryParse(numericString, out _);
        }

        [UsedImplicitly]
        public static DateTime StartOfDayTime(this DateTime dateTime)
        {
            var afterMidnight = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, DateTimeKind.Utc);

            return afterMidnight;
        }

        [UsedImplicitly]
        public static DateTime EndOfDayTime(this DateTime dateTime)
        {
            var midnight = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, DateTimeKind.Utc);

            return midnight;
        }
    }
}
