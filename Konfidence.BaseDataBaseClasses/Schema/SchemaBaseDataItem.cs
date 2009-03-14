using System.Data;

namespace Konfidence.BaseData.Schema
{
    public partial class SchemaBaseDataItem: BaseDataItem
    {
        protected internal DataTable GetSchemaObject(string objectType)
        {
            BaseHost dataHost = GetHost();

            return dataHost.GetSchemaObject(objectType);
        }
    }
}
