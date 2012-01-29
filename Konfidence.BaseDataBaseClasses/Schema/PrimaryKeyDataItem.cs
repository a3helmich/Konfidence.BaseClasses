using System;
using System.Collections.Generic;
using System.Text;

namespace Konfidence.BaseData.Schema
{
    class PrimaryKeyDataItem: SchemaBaseDataItem
    {
        // field definitions
        private const string CONSTRAINT_NAME = "Constraint_Name";
        private const string PRIMARYKEYID = "PrimaryKeyId";

        private const string TABLENAME = "tableName";

        private string _ConstraintName = string.Empty;
        private string _ColumnName = string.Empty;
        private string _DataType = string.Empty;

        #region properties
        public int PrimaryKeyId
        {
            get { return Id; }
        }

        public string ConstraintName
        {
            get { return _ConstraintName; }
            set { _ConstraintName = value; }
        }

        public string ColumnName
        {
            get { return _ColumnName; }
            set { _ColumnName = value; }
        }

        public string DataType
        {
            get { return _DataType; }
            set { _DataType = value; }
        }
        #endregion properties

        public PrimaryKeyDataItem()
        {
        }

        public PrimaryKeyDataItem(string tableName): this()
        {
            SetParameter(TABLENAME, tableName);

            GetItem(SPNames.PRIMARYKEY_GET);
        }

        protected override void InitializeDataItem()
        {
            AutoIdField = PRIMARYKEYID;
        }

        protected internal override void GetData()
        {
            _ConstraintName = GetFieldString(CONSTRAINT_NAME);
        }
    }
}
