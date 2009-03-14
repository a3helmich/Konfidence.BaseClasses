using System;
using System.Collections.Generic;
using System.Text;

namespace Konfidence.BaseData.Schema.Helper
{
    public class TableGetByFieldItem
    {
        private string _TableName = string.Empty;
        private string _FieldName = string.Empty;

        #region properties
        public string TableName
        {
            get { return _TableName; }
        }

        public string FieldName
        {
            get { return _FieldName; }
        }
        #endregion properties

        public TableGetByFieldItem(string tableName, string fieldName)
        {
            _TableName = tableName.Trim();
            _FieldName = fieldName.Trim();
        }
    }
}
