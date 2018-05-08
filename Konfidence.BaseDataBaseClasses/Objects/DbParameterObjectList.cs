using System;
using System.Collections.Generic;
using System.Data;
using Konfidence.BaseDataInterfaces;

namespace Konfidence.BaseData.Objects
{
    public class DbParameterObjectList : List<IDbParameterObject>, IDbParameterObjectList
    {
        public void SetField(string fieldName, int value)
        {
            AddInParameter(fieldName, DbType.Int32, value);
        }

        public void SetField(string fieldName, byte value)
        {
            AddInParameter(fieldName, DbType.Byte, value);
        }

        public void SetField(string fieldName, short value)
        {
            AddInParameter(fieldName, DbType.Int16, value);
        }

        public void SetField(string fieldName, long value)
        {
            AddInParameter(fieldName, DbType.Int64, value);
        }

        public void SetField(string fieldName, Guid value)
        {
            if (Guid.Empty.Equals(value))
            {
                AddInParameter(fieldName, DbType.Guid, null);
            }
            else
            {
                AddInParameter(fieldName, DbType.Guid, value);
            }
        }

        public void SetField(string fieldName, string value)
        {
            AddInParameter(fieldName, DbType.String, value);
        }

        public void SetField(string fieldName, bool value)
        {
            AddInParameter(fieldName, DbType.Boolean, value);
        }

        public void SetField(string fieldName, DateTime value)
        {
            if (value > DateTime.MinValue)
            {
                AddInParameter(fieldName, DbType.DateTime, value);
            }
            else
            {
                AddInParameter(fieldName, DbType.DateTime, null);
            }
        }

        public void SetField(string fieldName, TimeSpan value)
        {
            if (value > TimeSpan.MinValue)
            {
                var inbetween = DateTime.Now;

                inbetween = new DateTime(inbetween.Year, inbetween.Month, inbetween.Day, value.Hours, value.Minutes, value.Seconds, value.Milliseconds);

                AddInParameter(fieldName, DbType.Time, inbetween);
            }
            else
            {
                AddInParameter(fieldName, DbType.Time, null);
            }
        }

        public void SetField(string fieldName, decimal value)
        {
            AddInParameter(fieldName, DbType.Decimal, value);
        }

        private void AddInParameter(string field, DbType dbType, object value)
        {
            Add(new DbParameterObject(field, dbType, value));
        }
    }
}
