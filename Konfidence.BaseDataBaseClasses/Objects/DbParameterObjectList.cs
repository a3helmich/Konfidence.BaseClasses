using System;
using System.Collections.Generic;
using System.Data;
using JetBrains.Annotations;
using Konfidence.DataBaseInterface;

namespace Konfidence.BaseData.Objects
{
    internal static class DbParamExtensionMethods
    {
        [NotNull] private static readonly Dictionary<Type, DbType> _typeMap = new Dictionary<Type, DbType>();

        static DbParamExtensionMethods()
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
            //typeMap[typeof(System.Data.Linq.Binary)] = DbType.Binary;
        }

        public static void SetField<T>([NotNull] this List<IDbParameterObject> dbParameterObjects, string fieldName, T value) where T : Type
        {
            dbParameterObjects.AddInParameter(fieldName, _typeMap[typeof(T)], value);
        }

        public static void SetField([NotNull] this List<IDbParameterObject> dbParameterObjects, string fieldName, int value)
        {
            dbParameterObjects.AddInParameter(fieldName, DbType.Int32, value);
        }

        public static void SetField([NotNull] this List<IDbParameterObject> dbParameterObjects, string fieldName, byte value)
        {
            dbParameterObjects.AddInParameter(fieldName, DbType.Byte, value);
        }

        public static void SetField([NotNull] this List<IDbParameterObject> dbParameterObjects, string fieldName, short value)
        {
            dbParameterObjects.AddInParameter(fieldName, DbType.Int16, value);
        }

        public static void SetField([NotNull] this List<IDbParameterObject> dbParameterObjects, string fieldName, long value)
        {
            dbParameterObjects.AddInParameter(fieldName, DbType.Int64, value);
        }

        public static void SetField([NotNull] this List<IDbParameterObject> dbParameterObjects, string fieldName, Guid value)
        {
            if (Guid.Empty.Equals(value))
            {
                dbParameterObjects.AddInParameter(fieldName, DbType.Guid, null);
            }
            else
            {
                dbParameterObjects.AddInParameter(fieldName, DbType.Guid, value);
            }
        }

        public static void SetField([NotNull] this List<IDbParameterObject> dbParameterObjects, string fieldName, string value)
        {
            dbParameterObjects.AddInParameter(fieldName, DbType.String, value);
        }

        public static void SetField([NotNull] this List<IDbParameterObject> dbParameterObjects, string fieldName, bool value)
        {
            dbParameterObjects.AddInParameter(fieldName, DbType.Boolean, value);
        }

        public static void SetField([NotNull] this List<IDbParameterObject> dbParameterObjects, string fieldName, DateTime value)
        {
            if (value > DateTime.MinValue)
            {
                dbParameterObjects.AddInParameter(fieldName, DbType.DateTime, value);
            }
            else
            {
                dbParameterObjects.AddInParameter(fieldName, DbType.DateTime, null);
            }
        }

        public static void SetField([NotNull] this List<IDbParameterObject> dbParameterObjects, string fieldName, TimeSpan value)
        {
            if (value > TimeSpan.MinValue)
            {
                var inbetween = DateTime.Now;

                inbetween = new DateTime(inbetween.Year, inbetween.Month, inbetween.Day, value.Hours, value.Minutes, value.Seconds, value.Milliseconds);

                dbParameterObjects.AddInParameter(fieldName, DbType.Time, inbetween);
            }
            else
            {
                dbParameterObjects.AddInParameter(fieldName, DbType.Time, null);
            }
        }

        public static void SetField([NotNull] this List<IDbParameterObject> dbParameterObjects, string fieldName, decimal value)
        {
            dbParameterObjects.AddInParameter(fieldName, DbType.Decimal, value);
        }

        private static void AddInParameter([NotNull] this List<IDbParameterObject> dbParameterObjects, string field, DbType dbType, object value)
        {
            dbParameterObjects.Add(new DbParameterObject(field, dbType, value));
        }
    }

    //public class DbParameterObjectList : List<IDbParameterObject>, IDbParameterObjectList
    //{

    //    public void SetField(string fieldName, int value)
    //    {
    //        AddInParameter(fieldName, DbType.Int32, value);
    //    }

    //    public void SetField(string fieldName, byte value)
    //    {
    //        AddInParameter(fieldName, DbType.Byte, value);
    //    }

    //    public void SetField(string fieldName, short value)
    //    {
    //        AddInParameter(fieldName, DbType.Int16, value);
    //    }

    //    public void SetField(string fieldName, long value)
    //    {
    //        AddInParameter(fieldName, DbType.Int64, value);
    //    }

    //    public void SetField(string fieldName, Guid value)
    //    {
    //        if (Guid.Empty.Equals(value))
    //        {
    //            AddInParameter(fieldName, DbType.Guid, null);
    //        }
    //        else
    //        {
    //            AddInParameter(fieldName, DbType.Guid, value);
    //        }
    //    }

    //    public void SetField(string fieldName, string value)
    //    {
    //        AddInParameter(fieldName, DbType.String, value);
    //    }

    //    public void SetField(string fieldName, bool value)
    //    {
    //        AddInParameter(fieldName, DbType.Boolean, value);
    //    }

    //    public void SetField(string fieldName, DateTime value)
    //    {
    //        if (value > DateTime.MinValue)
    //        {
    //            AddInParameter(fieldName, DbType.DateTime, value);
    //        }
    //        else
    //        {
    //            AddInParameter(fieldName, DbType.DateTime, null);
    //        }
    //    }

    //    public void SetField(string fieldName, TimeSpan value)
    //    {
    //        if (value > TimeSpan.MinValue)
    //        {
    //            var inbetween = DateTime.Now;

    //            inbetween = new DateTime(inbetween.Year, inbetween.Month, inbetween.Day, value.Hours, value.Minutes, value.Seconds, value.Milliseconds);

    //            AddInParameter(fieldName, DbType.Time, inbetween);
    //        }
    //        else
    //        {
    //            AddInParameter(fieldName, DbType.Time, null);
    //        }
    //    }

    //    public void SetField(string fieldName, decimal value)
    //    {
    //        AddInParameter(fieldName, DbType.Decimal, value);
    //    }

    //    private void AddInParameter(string field, DbType dbType, object value)
    //    {
    //        Add(new DbParameterObject(field, dbType, value));
    //    }
    //}
}
