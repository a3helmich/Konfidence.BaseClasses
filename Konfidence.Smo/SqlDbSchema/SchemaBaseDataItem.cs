using System.Data;
using Konfidence.BaseData;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public class SchemaBaseDataItem : BaseDataItem
    {
        protected DataTable GetSchemaObject(string objectType)
        {
            // TODO : enable again
            return DataHost.GetSchemaObject(objectType);
        }
    }
}
