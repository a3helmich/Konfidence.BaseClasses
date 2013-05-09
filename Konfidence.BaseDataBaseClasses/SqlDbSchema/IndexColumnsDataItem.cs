using System.Data;

namespace Konfidence.BaseData.SqlDbSchema
{
    public class IndexColumnsDataItem : SchemaBaseDataItem
    {
        public DataTable GetIndexedColumns()
        {
            return GetSchemaObject("IndexColumns");
        }
    }
}
