using System.Data;
using Konfidence.BaseData;
using Konfidence.BaseDataInterfaces;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public class SchemaBaseDataItem : BaseDataItem
    {
        protected DataTable GetSchemaObject(string objectType)
        {
            // TODO : enable again
            //return Client.GetSchemaObject(objectType);
            return null;
        }

        public override IBaseClient ClientBind()
        {
            // TODO : enable
            //return base.ClientBind<SqlClient>();
            return null;
        }
    }
}
