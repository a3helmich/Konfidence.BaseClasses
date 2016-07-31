using System.Data;

namespace Konfidence.Smo.SqlDbSchema
{
    public class IndexColumnsDataItem : SchemaBaseDataItem
    {
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
