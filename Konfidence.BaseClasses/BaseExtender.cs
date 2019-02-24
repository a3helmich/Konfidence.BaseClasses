﻿using System;
using JetBrains.Annotations;

namespace Konfidence.Base
{
    [UsedImplicitly]
    public static class BaseExtender
    {
        [ContractAnnotation("assignedObject:null => false")]
        public static bool IsAssigned(this object assignedObject)
        {
            if (assignedObject is string assignedString)
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
            if (Guid.TryParse(assignedGuid, out var _))
            {
                return true;
            }

            return false;
        }

        [ContractAnnotation("assignedGuid:null => false")]
        public static bool IsAssigned(this Guid assignedGuid)
        {
            return !Guid.Empty.Equals(assignedGuid);
        }

        [ContractAnnotation("line:null => true")]
        public static bool IsEof(this string line)
        {
            return line == null;
        }
    }
}
