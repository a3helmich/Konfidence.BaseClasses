using System;
using System.Data;

namespace Konfidence.BaseData.SqlDbSchema
{
    public class TableDataItemList : BaseDataItemList<TableDataItem> 
    {
        public TableDataItemList(string dataBaseName)
        {
            DataBaseName = dataBaseName;

            var dataTableList = GetTables();

            BuildItemList(dataTableList);
        }

        internal class InternalTableDataItem : SchemaBaseDataItem
        {
            internal DataTable GetTables()
            {
                return GetSchemaObject("Tables");
            }
        }

        private DataTable GetTables()
        {
            var shemaBaseDataItem = new InternalTableDataItem();

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

                        if (IsAssigned(name) && (!name.ToLower().Equals("dtproperties") && !name.ToLower().StartsWith("sys")))
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
