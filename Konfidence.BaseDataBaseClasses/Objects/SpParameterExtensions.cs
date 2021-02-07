using System;
using System.Collections.Generic;
using System.Data;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.DataBaseInterface;

namespace Konfidence.BaseData.Objects
{
    public static class SpParameterExtensions
    {
        [NotNull] private static readonly Dictionary<Type, DbType> _typeMap = new();

        static SpParameterExtensions()
        {
            _typeMap[typeof(byte)] = DbType.Byte;
            _typeMap[typeof(sbyte)] = DbType.SByte;
            _typeMap[typeof(short)] = DbType.Int16;
            _typeMap[typeof(ushort)] = DbType.UInt16;
            _typeMap[typeof(int)] = DbType.Int32;
            _typeMap[typeof(uint)] = DbType.UInt32;
            _typeMap[typeof(long)] = DbType.Int64;
            _typeMap[typeof(ulong)] = DbType.UInt64;
            _typeMap[typeof(float)] = DbType.Single;
            _typeMap[typeof(double)] = DbType.Double;
            _typeMap[typeof(decimal)] = DbType.Decimal;
            _typeMap[typeof(bool)] = DbType.Boolean;
            _typeMap[typeof(string)] = DbType.String;
            _typeMap[typeof(char)] = DbType.StringFixedLength;
            _typeMap[typeof(Guid)] = DbType.Guid;
            _typeMap[typeof(DateTime)] = DbType.DateTime;
            _typeMap[typeof(DateTimeOffset)] = DbType.DateTimeOffset;
            _typeMap[typeof(TimeSpan)] = DbType.Time;
            _typeMap[typeof(byte[])] = DbType.Binary;
            _typeMap[typeof(byte?)] = DbType.Byte;
            _typeMap[typeof(sbyte?)] = DbType.SByte;
            _typeMap[typeof(short?)] = DbType.Int16;
            _typeMap[typeof(ushort?)] = DbType.UInt16;
            _typeMap[typeof(int?)] = DbType.Int32;
            _typeMap[typeof(uint?)] = DbType.UInt32;
            _typeMap[typeof(long?)] = DbType.Int64;
            _typeMap[typeof(ulong?)] = DbType.UInt64;
            _typeMap[typeof(float?)] = DbType.Single;
            _typeMap[typeof(double?)] = DbType.Double;
            _typeMap[typeof(decimal?)] = DbType.Decimal;
            _typeMap[typeof(bool?)] = DbType.Boolean;
            _typeMap[typeof(char?)] = DbType.StringFixedLength;
            _typeMap[typeof(Guid?)] = DbType.Guid;
            _typeMap[typeof(DateTime?)] = DbType.DateTime;
            _typeMap[typeof(DateTimeOffset?)] = DbType.DateTimeOffset;
            _typeMap[typeof(TimeSpan?)] = DbType.Time;
            //typeMap[typeof(System.Data.Linq.Binary)] = DbType.Binary;
        }

        public static void SetParameter<T>([NotNull] this List<ISpParameterData> spParameterData, string parameterName, T value) 
        {
            spParameterData.AddInParameter(parameterName, _typeMap[typeof(T)], value);
        }

        public static void SetParameter([NotNull] this List<ISpParameterData> spParameterData, string parameterName, Guid guidValue)
        {
            if (guidValue.IsAssigned())
            {
                spParameterData.AddInParameter(parameterName, _typeMap[typeof(Guid)], guidValue);

                return;
            }

            spParameterData.AddInParameter(parameterName, _typeMap[typeof(Guid)], null);
        }

        public static void SetParameter([NotNull] this List<ISpParameterData> spParameterData, string parameterName, DateTime dateTimeValue)
        {
            if (dateTimeValue.IsAssigned())
            {
                spParameterData.AddInParameter(parameterName, _typeMap[typeof(DateTime)], dateTimeValue);

                return;
            }

            spParameterData.AddInParameter(parameterName, _typeMap[typeof(DateTime)], null);
        }

        public static void SetParameter([NotNull] this List<ISpParameterData> spParameterData, string parameterName, TimeSpan timeSpan)
        {
            if (timeSpan.IsAssigned())
            {
                var dateTime = DateTime.Today;

                dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);

                spParameterData.AddInParameter(parameterName, _typeMap[typeof(TimeSpan)], dateTime);

                return;
            }

            spParameterData.AddInParameter(parameterName, _typeMap[typeof(TimeSpan)], null);
        }

        private static void AddInParameter([NotNull] this List<ISpParameterData> spParameterData, string field, DbType dbType, object value)
        {
            spParameterData.Add(new SpParameter(field, dbType, value));
        }
    }
}
