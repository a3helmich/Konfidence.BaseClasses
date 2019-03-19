using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

[assembly: InternalsVisibleTo("Konfidence.Security.Tests, PublicKey = 002400000c80000094000000060200000024000052534131000400000100010001e4cbf3ebe277" +
                              "6ba5fa278fb19d2c9bfea6a111c37d29fcbdc5fbb96194e0dde397cff409fb04afcbf7efe3182d" +
                              "dad89e4f82270db85102701831a12e299f0fddd21e071df3f28381c9d27db11b690fdc941362dc" +
                              "8f6b8cef4f4fce1c67c3314553eab8d7bf828a458add25d5af5949999efc27dd424f7608dbb224" +
                              "df4c61e3, PublicKeyToken=f9013a630227061d")]

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

        [UsedImplicitly]
        [ContractAnnotation("line:null => true")]
        public static bool IsEof(this string line)
        {
            return line == null;
        }

        [ContractAnnotation("assignedGuid:null => false")]
        public static bool IsGuid(this string assignedGuid)
        {
            return Guid.TryParse(assignedGuid, out var _);
        }

        [ContractAnnotation("assignedGuid:null => false")]
        public static bool IsAssigned(this Guid assignedGuid)
        {
            return !Guid.Empty.Equals(assignedGuid);
        }
    }
}
