using System;
using System.Data;
using Konfidence.Base;
using Konfidence.BaseData;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public class TableDataItemList : BaseDataItemList<TableDataItem> 
    {
        public TableDataItemList(string databaseName)
        {
            DatabaseName = databaseName;

            var dataTableList = GetTables();

            BuildItemList(dataTableList);
        }

        internal class InternalTableDataItem : SchemaBaseDataItem
        {
            public InternalTableDataItem(string databaseName)
            {
                DatabaseName = databaseName;
            }

            internal DataTable GetTables()
            {
                return GetSchemaObject("Tables");
            }
        }

        private DataTable GetTables()
        {
            var shemaBaseDataItem = new InternalTableDataItem(DatabaseName);

            return shemaBaseDataItem.GetTables();
        }

        private void BuildItemList(DataTable dataTableList)
        {
            foreach (DataRow dataRow in dataTableList.Rows)
            {
                var tableType = dataRow["TABLE_TYPE"] as string;

                switch (tableType)
                {
                    case "BASE TABLE":

                        var catalog = dataRow["TABLE_CATALOG"] as string;
                        var schema = dataRow["TABLE_SCHEMA"] as string;
                        var name = dataRow["TABLE_NAME"] as string;

                        if (name.IsAssigned() && (!name.Equals("dtproperties", StringComparison.OrdinalIgnoreCase) && !name.StartsWith("sys",StringComparison.OrdinalIgnoreCase)))
                        {
                            var tableDataItem = new TableDataItem(catalog, schema, name);

                            Add(tableDataItem);
                        }

                        break;
                    case "VIEW":
                        break;
                    default:
                        throw new Exception("onbekend tabletype");
                }
            }
        }
    }
}
