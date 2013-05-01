using System;
using System.Data;
using Konfidence.Base;

namespace Konfidence.BaseData.ParameterObjects
{
    public class DbParameterObjectList : BaseItemList<DbParameterObject>
    {
        #region SetField Methods
        internal void SetField(string fieldName, int value)
        {
            AddInParameter(fieldName, DbType.Int32, value);
        }

        internal void SetField(string fieldName, Guid value)
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

        internal void SetField(string fieldName, string value)
        {
            AddInParameter(fieldName, DbType.String, value);
        }

        internal void SetField(string fieldName, bool value)
        {
            AddInParameter(fieldName, DbType.Boolean, value);
        }

        internal void SetField(string fieldName, DateTime value)
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

        internal void SetField(string fieldName, TimeSpan value)
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

        internal void SetField(string fieldName, Decimal value)
        {
            AddInParameter(fieldName, DbType.Decimal, value);
        }
        #endregion

        private void AddInParameter(string field, DbType dbType, object value)
        {
            Add(new DbParameterObject(field, dbType, value));
        }
    }
}
