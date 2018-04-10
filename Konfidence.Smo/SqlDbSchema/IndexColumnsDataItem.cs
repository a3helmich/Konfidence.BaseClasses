using System.Data;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public class IndexColumnsDataItem : SchemaBaseDataItem
    {
        public IndexColumnsDataItem() { }

        public IndexColumnsDataItem(string databaseName)
        {
            DatabaseName = databaseName;
        }

        public DataTable GetIndexedColumns()
        {
            return GetSchemaObject("IndexColumns");
        }
    }
}
