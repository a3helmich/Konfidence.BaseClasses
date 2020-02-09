using System;
using System.Collections.Generic;
using System.Data;
using JetBrains.Annotations;
using Konfidence.DataBaseInterface;

namespace Konfidence.BaseDatabaseClasses.Objects
{
    internal static class DbParameterObjectExtensions
    {
        [NotNull] private static readonly Dictionary<Type, DbType> _typeMap = new Dictionary<Type, DbType>();

        static DbParameterObjectExtensions()
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

        public static void SetParameter<T>([NotNull] this List<IDbParameterObject> dbParameterObjects, string parameterName, T value) 
        {
            dbParameterObjects.AddInParameter(parameterName, _typeMap[typeof(T)], value);
        }

        public static void SetParameter([NotNull] this List<IDbParameterObject> dbParameterObjects, string parameterName, Guid value)
        {
            if (Guid.Empty.Equals(value))
            {
                dbParameterObjects.AddInParameter(parameterName, _typeMap[typeof(Guid)], null);
            }
            else
            {
                dbParameterObjects.AddInParameter(parameterName, _typeMap[typeof(Guid)], value);
            }
        }

        public static void SetParameter([NotNull] this List<IDbParameterObject> dbParameterObjects, string parameterName, DateTime value)
        {
            if (value > DateTime.MinValue)
            {
                dbParameterObjects.AddInParameter(parameterName, _typeMap[typeof(DateTime)], value);
            }
            else
            {
                dbParameterObjects.AddInParameter(parameterName, _typeMap[typeof(DateTime)], null);
            }
        }

        public static void SetParameter([NotNull] this List<IDbParameterObject> dbParameterObjects, string parameterName, TimeSpan value)
        {
            if (value > TimeSpan.MinValue)
            {
                var timeSpan = DateTime.Today;

                timeSpan = new DateTime(timeSpan.Year, timeSpan.Month, timeSpan.Day, value.Hours, value.Minutes, value.Seconds, value.Milliseconds);

                dbParameterObjects.AddInParameter(parameterName, _typeMap[typeof(TimeSpan)], timeSpan);
            }
            else
            {
                dbParameterObjects.AddInParameter(parameterName, _typeMap[typeof(TimeSpan)], null);
            }
        }

        private static void AddInParameter([NotNull] this List<IDbParameterObject> dbParameterObjects, string field, DbType dbType, object value)
        {
            dbParameterObjects.Add(new DbParameterObject(field, dbType, value));
        }
    }
}
