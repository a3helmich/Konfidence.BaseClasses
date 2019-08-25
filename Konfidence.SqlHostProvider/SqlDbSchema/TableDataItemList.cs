using System;
using System.Data;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.BaseData;
using Konfidence.BaseDataInterfaces;
using Konfidence.SqlHostProvider.SqlAccess;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public class TableDataItemList : BaseDataItemList<TableDataItem> 
    {
        public TableDataItemList(string connectionName)
        {
            ConnectionName = connectionName;

            var dataTableList = GetTables();

            BuildItemList(dataTableList);
        }

        [NotNull]
        protected override IBaseClient ClientBind()
        {
            return base.ClientBind<SqlClient>();
        }

        internal class InternalTableDataItem : SchemaBaseDataItem
        {
            public InternalTableDataItem(string connectionName)
            {
                ConnectionName = connectionName;
            }

            internal DataTable GetTables()
            {
                return GetSchemaObject("Tables");
            }
        }

        private DataTable GetTables()
        {
            var shemaBaseDataItem = new InternalTableDataItem(ConnectionName);

            return shemaBaseDataItem.GetTables();
        }

        private void BuildItemList([NotNull] DataTable dataTableList)
        {
            foreach (DataRow dataRow in dataTableList.Rows)
            {
                var tableType = dataRow["TABLE_TYPE"] as string;

                switch (tableType)
                {
                    case "BASE TABLE":

                        var catalog = dataRow["TABLE_CATALOG"] as string;
                        //var schema = dataRow["TABLE_SCHEMA"] as string;
                        var name = dataRow["TABLE_NAME"] as string;

                        if (name.IsAssigned() && (!name.Equals("dtproperties", StringComparison.OrdinalIgnoreCase) && !name.StartsWith("sys",StringComparison.OrdinalIgnoreCase)))
                        {
                            var tableDataItem = new TableDataItem(ConnectionName, catalog, name);

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
