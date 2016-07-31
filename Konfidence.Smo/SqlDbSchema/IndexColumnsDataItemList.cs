using System.Data;
using Konfidence.Base;

namespace Konfidence.BaseData.SqlDbSchema
{
    public class IndexColumnsDataItemList : BaseDataItemList<IndexColumnsDataItem>
    {
        private readonly string _tableName;

        public PrimaryKeyDataItem PrimaryKeyDataItem { get; }

        public IndexColumnsDataItemList(string databaseName, string tableName)
        {
            _tableName = tableName;

            DatabaseName = databaseName;

            PrimaryKeyDataItem = new PrimaryKeyDataItem(DatabaseName, _tableName);

            var indexColumnDataItem = new IndexColumnsDataItem(DatabaseName);

            var dataTable = indexColumnDataItem.GetIndexedColumns();

            BuildItemList(dataTable);
        }

        private void BuildItemList(DataTable dataTable)
        {
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var tableName = dataRow["TABLE_NAME"] as string;

                if (tableName.IsAssigned() && tableName.Equals(_tableName))
                {
                    var columnName = dataRow["COLUMN_NAME"] as string;
                    var constraintName = dataRow["CONSTRAINT_NAME"] as string;

                    var indexColumnDataItem = new IndexColumnsDataItem(DatabaseName);

                    if (constraintName.IsAssigned() && constraintName.Equals(PrimaryKeyDataItem.ConstraintName))
                    {
                        PrimaryKeyDataItem.ColumnName = columnName;
                    }

                    Add(indexColumnDataItem);
                }
            }
        }
    }
}
