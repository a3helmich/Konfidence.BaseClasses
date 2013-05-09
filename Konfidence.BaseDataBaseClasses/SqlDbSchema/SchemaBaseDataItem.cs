using System.Data;

namespace Konfidence.BaseData.SqlDbSchema
{
    public class SchemaBaseDataItem : BaseDataItem
    {
        protected DataTable GetSchemaObject(string objectType)
        {
            return DataHost.GetSchemaObject(objectType);
        }
    }
}
