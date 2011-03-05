using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Konfidence.BaseData.Schema
{
    public class ColumnDataItemList : BaseDataItemList<ColumnDataItem>
    {
        //private DataTable _DataTable;
        private string _TableName;
        private bool _StoredProcedureCreated = false;

        private const string COLUMNS_GETLIST = "Columns_GetList_gen";

        private string _CreateStoredProcedureCommand = "CREATE PROCEDURE [dbo].[Columns_GetList_gen] @tableName varchar(50) AS BEGIN SET NOCOUNT ON; SELECT t.name as tableName, st.name as datatype, cc.* FROM sys.columns cc, sys.tables t, sys.systypes st where cc.object_id = t.object_id and t.name = @tableName and st.xtype = cc.system_type_id END";
        private string _DeleteStoredProcedureCommand = "DROP PROCEDURE [dbo].[Columns_GetList_gen]";

        protected override void InitializeDataItemList()
        {
            GetListStoredProcedure = COLUMNS_GETLIST;
        }

        public ColumnDataItemList(string tableName) : base()
        {
            _TableName = tableName;

            CreateSchemaCommand();

            BuildItemList();

            DeleteSchemaCommand();
        }

        public override void SetParameters(string storedProcedure, Database database, DbCommand dbCommand)
        {

            if (storedProcedure.Equals(COLUMNS_GETLIST))
            {
                base.SetParameters(storedProcedure, database, dbCommand);

                database.AddInParameter(dbCommand, "TableName", DbType.String, _TableName);
            }
        }

        private void CreateSchemaCommand()
        {
            if (!StoredProcedureExists(COLUMNS_GETLIST))
            {
                ExecuteTextCommand(_CreateStoredProcedureCommand);

                _StoredProcedureCreated = true;
            }
        }

        private void DeleteSchemaCommand()
        {
            if (_StoredProcedureCreated)
            {
                ExecuteTextCommand(_DeleteStoredProcedureCommand);
            }
        }

        protected override ColumnDataItem GetNewDataItem()
        {
            return new ColumnDataItem();
        }

        public ColumnDataItem Find(string columnName)
        {
            foreach (ColumnDataItem columnDataItem in this)
            {
                if (columnDataItem.Name.Equals(columnName))
                {
                    return columnDataItem;
                }
            }

            return null;
        }

        //private void BuildItemList(DataTable dataTable)
        //{
        //    foreach (DataRow dataRow in dataTable.Rows)
        //    {
        //        string tableName = dataRow["TABLE_NAME"] as string;
        //        ColumnDataItem columnDataItem = null;

        //        if (tableName.Equals(_TableName))
        //        {
        //            string columnName = dataRow["COLUMN_NAME"] as string;

        //            columnDataItem = new ColumnDataItem(columnName);

        //            int ordinalPosition = (int)dataRow["ORDINAL_POSITION"];
        //            string columnDefault = dataRow["COLUMN_DEFAULT"] as string;
        //            string dataType = dataRow["DATA_TYPE"] as string;
        //            string characterMaximumLength = string.Empty;
        //            if (dataType.Equals("varchar") || dataType.Equals("char"))
        //            {
        //                int charLength = (int)dataRow["CHARACTER_MAXIMUM_LENGTH"];
        //                characterMaximumLength = charLength.ToString();
        //            }

        //            columnDataItem.OrdinalPosition = ordinalPosition;
        //            columnDataItem.ColumnDefault = columnDefault;
        //            columnDataItem.DataType = dataType;
        //            columnDataItem.CharacterMaximumLength = characterMaximumLength;

        //            /*
        //            bool isNullable = (bool)dataRow["IS_NULLABLE"];
        //            int characterMaximumLength = (int)dataRow["CHARACTER_MAXIMUM_LENGTH"];
        //            int characterOctetLength = (int)dataRow["CHARACTER_OCTET_LENGTH"];
        //            int numericPrecision = (int)dataRow["NUMERIC_PRECISION"];
        //            int numericPrecisionRadix = (int)dataRow["NUMERIC_PRECISION_RADIX"];
        //            int numericScale = (int)dataRow["NUMERIC_SCALE"];
        //            int dateTimePrecision = (int)dataRow["DATETIME_PRECISION"];
        //            string characterSetCatalog = dataRow["CHARACTER_SET_CATALOG"] as string;
        //            string characterSetSchema = dataRow["CHARACTER_SET_SCHEMA"] as string;
        //            string characterSetName = dataRow["CHARACTER_SET_NAME"] as string;
        //            string collationCatalog = dataRow["COLLATION_CATALOG"] as string;

        //            columnDataItem.IsNullable = isNullable;
        //            columnDataItem.CharacterOctetLength = characterOctetLength;
        //            columnDataItem.NumericPrecision = numericPrecision;
        //            columnDataItem.NumericPrecisionRadix = numericPrecisionRadix;
        //            columnDataItem.DateTimePrecision = dateTimePrecision;
        //            columnDataItem.CharacterSetCatalog = characterSetCatalog;
        //            columnDataItem.CharacterSetSchema = characterSetSchema;
        //            columnDataItem.CharacterSetName = characterSetName;
        //            columnDataItem.CollationCatalog = collationCatalog;
        //            */

        //            this.Add(columnDataItem);
        //        }
        //    }
        //}

    }
}
