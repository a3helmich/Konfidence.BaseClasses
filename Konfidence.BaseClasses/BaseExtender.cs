using System;
using JetBrains.Annotations;

namespace Konfidence.Base
{
    public static class BaseExtender
    {
        [ContractAnnotation("assignedObject:null => false")]
        public static bool IsAssigned(this object assignedObject)
        {
            var assignedString = assignedObject as string;

            if (assignedString != null)
            {
                if (string.IsNullOrWhiteSpace(assignedString))
                {
                    return false;
                }

                return true;
            }

            if (assignedObject == null)
            {
                return false;
            }

            return true;
        }
        public static bool IsAssigned(this DateTime assignedTime)
        {
            if (assignedTime > DateTime.MinValue)
            {
                return true;
            }

            return false;
        }
        public static DateTime StartOfDayTime(this DateTime dateTime)
        {
            var afterMidnight = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, DateTimeKind.Utc);

            return afterMidnight;
        }

        public static DateTime EndOfDayTime(this DateTime dateTime)
        {
            var midnight = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, DateTimeKind.Utc);

            return midnight;
        }

        [ContractAnnotation("assignedGuid:null => false")]
        public static bool IsGuid(this string assignedGuid)
        {
            Guid isGuid;

            if (Guid.TryParse(assignedGuid, out isGuid))
            {
                return true;
            }

            return false;
        }
    }
}
