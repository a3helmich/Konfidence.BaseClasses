using System.Data;
using JetBrains.Annotations;
using Konfidence.BaseData;
using Konfidence.DataBaseInterface;
using Konfidence.SqlHostProvider.SqlAccess;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public class SchemaBaseDataItem : BaseDataItem
    {
        protected DataTable GetSchemaObject(string objectType)
        {

            return Client.GetSchemaObject(objectType);
        }

        [NotNull]
        protected override IBaseClient ClientBind()
        {
            return base.ClientBind<SqlClient>();
        }
    }
}
