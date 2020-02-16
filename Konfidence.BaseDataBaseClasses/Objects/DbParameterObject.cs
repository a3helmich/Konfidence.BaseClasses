using System.Data;
using Konfidence.DataBaseInterface;

namespace Konfidence.BaseDatabaseClasses.Objects
{
    internal class DbParameterObject : IDbParameterObject
    {
        public DbParameterObject(string parameterName, DbType dbType, object value)
        {
            ParameterName = parameterName;
            DbType = dbType;
            Value = value;
        }

        public string ParameterName { get; set; }

        public DbType DbType { get; set; }

        public object Value { get; set; }
    }
}
