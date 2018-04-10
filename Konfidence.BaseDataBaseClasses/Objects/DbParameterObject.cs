using System.Data;
using Konfidence.BaseDataInterfaces;

namespace Konfidence.BaseData.Objects
{
    public class DbParameterObject : BaseDataItem, IDbParameterObject
    {
        public DbParameterObject() { }

        public DbParameterObject(string field, DbType dbType, object value)
        {
            Field = field;
            DbType = dbType;
            Value = value;
        }

        public string Field { get; set; }

        public DbType DbType { get; set; }

        public object Value { get; set; }
    }
}
