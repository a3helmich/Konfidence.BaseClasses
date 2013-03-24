using System.Data;

namespace Konfidence.BaseData
{
    public class SchemaBaseDataItem : BaseDataItem
    {
        protected DataTable GetSchemaObject(string objectType)
        {
            BaseHost dataHost = GetHost();

            return dataHost.GetSchemaObject(objectType);
        }
    }
}
