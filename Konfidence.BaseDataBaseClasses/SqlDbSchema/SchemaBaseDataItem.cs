using System.Data;

namespace Konfidence.BaseData.SqlDbSchema
{
    internal class SchemaBaseDataItem : BaseDataItem
    {
        protected DataTable GetSchemaObject(string objectType)
        {
            return DataHost.GetSchemaObject(objectType);
        }
    }
}
