using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Konfidence.BaseData.Schema
{
    public class IndexColumnsDataItemList: BaseDataItemList<IndexColumnsDataItem>
    {
        private DataTable _DataTable;
        private string _TableName;
        private PrimaryKeyDataItem _PrimaryKeyDataItem = null;
        
        #region properties
        internal PrimaryKeyDataItem PrimaryKeyDataItem
        {
            get { return _PrimaryKeyDataItem; }
        }
        #endregion properties

        public IndexColumnsDataItemList(string tableName)
        {
            _TableName = tableName;

            _PrimaryKeyDataItem = new PrimaryKeyDataItem(_TableName);

            BuildItemList(_DataTable);
        }

        protected override void InitializeDataItemList()
        {
            IndexColumnsDataItem indexColumnDataItem = GetNewDataItem();

            _DataTable = indexColumnDataItem.GetSchemaObject("IndexColumns");
        }

        private void BuildItemList(DataTable dataTable)
        {
            foreach (DataRow dataRow in dataTable.Rows)
            {
                string tableName = dataRow["TABLE_NAME"] as string;
                IndexColumnsDataItem indexColumnDataItem = null;

                if (tableName.Equals(_TableName))
                {
                    string columnName = dataRow["COLUMN_NAME"] as string;
                    string constraintName = dataRow["CONSTRAINT_NAME"] as string;

                    indexColumnDataItem = new IndexColumnsDataItem(columnName, constraintName);

                    if (constraintName.Equals(_PrimaryKeyDataItem.ConstraintName))
                    {
                        _PrimaryKeyDataItem.ColumnName = columnName;
                    }

                    this.Add(indexColumnDataItem);
                }
            }
        }

        protected override IndexColumnsDataItem GetNewDataItem()
        {
            return new IndexColumnsDataItem();
        }

    }
}
