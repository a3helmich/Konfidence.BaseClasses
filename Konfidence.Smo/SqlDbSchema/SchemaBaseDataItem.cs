using System.Data;
using Konfidence.BaseData;
using Konfidence.BaseDataInterfaces;
using Konfidence.SqlHostProvider.SqlAccess;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public class SchemaBaseDataItem : BaseDataItem
    {
        protected DataTable GetSchemaObject(string objectType)
        {
            return Client.GetSchemaObject(objectType);
        }

        protected override IBaseClient ClientBind()
        {
            return base.ClientBind<SqlClient>();
        }
    }
}
