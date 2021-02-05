using System.Data;
using Konfidence.DataBaseInterface;

namespace Konfidence.BaseData.Objects
{
    internal class DbParameterObject : IDbParameterObject
    {
        public string ParameterName { get; set; }

        public DbType DbType { get; set; }

        public object Value { get; set; }

        public DbParameterObject(string parameterName, DbType dbType, object value)
        {
            ParameterName = parameterName;
            DbType = dbType;
            Value = value;
        }
    }
}
