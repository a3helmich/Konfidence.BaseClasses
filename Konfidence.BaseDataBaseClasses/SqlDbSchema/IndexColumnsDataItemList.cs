using System.Data;
using Konfidence.Base;

namespace Konfidence.BaseData.SqlDbSchema
{
    public class IndexColumnsDataItemList : BaseDataItemList<IndexColumnsDataItem>
    {
        private readonly string _TableName;
        private readonly PrimaryKeyDataItem _PrimaryKeyDataItem;

        public PrimaryKeyDataItem PrimaryKeyDataItem
        {
            get { return _PrimaryKeyDataItem; }
        }

        public IndexColumnsDataItemList(string tableName)
        {
            _TableName = tableName;

            _PrimaryKeyDataItem = new PrimaryKeyDataItem(_TableName);

            var indexColumnDataItem = new IndexColumnsDataItem();

            var dataTable = indexColumnDataItem.GetIndexedColumns();

            BuildItemList(dataTable);
        }

        private void BuildItemList(DataTable dataTable)
        {
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var tableName = dataRow["TABLE_NAME"] as string;

                if (tableName.IsAssigned() && tableName.Equals(_TableName))
                {
                    var columnName = dataRow["COLUMN_NAME"] as string;
                    var constraintName = dataRow["CONSTRAINT_NAME"] as string;

                    var indexColumnDataItem = new IndexColumnsDataItem();

                    if (constraintName.IsAssigned() && constraintName.Equals(_PrimaryKeyDataItem.ConstraintName))
                    {
                        _PrimaryKeyDataItem.ColumnName = columnName;
                    }

                    Add(indexColumnDataItem);
                }
            }
        }
    }
}
