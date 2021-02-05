using System.Data;
using Konfidence.DataBaseInterface;

namespace Konfidence.BaseData.Objects
{
    internal class DbParameterData : IDbParameterData
    {
        public string ParameterName { get; set; }

        public DbType DbType { get; set; }

        public object Value { get; set; }

        public DbParameterData(string parameterName, DbType dbType, object value)
        {
            ParameterName = parameterName;
            DbType = dbType;
            Value = value;
        }
    }
}
