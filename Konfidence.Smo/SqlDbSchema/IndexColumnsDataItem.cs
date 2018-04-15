using System.Data;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public class IndexColumnsDataItem : SchemaBaseDataItem
    {
        public IndexColumnsDataItem() { }

        public IndexColumnsDataItem(string connectionName)
        {
            ConnectionName = connectionName;
        }

        public DataTable GetIndexedColumns()
        {
            return GetSchemaObject("IndexColumns");
        }
    }
}
