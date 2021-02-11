using System.Data;
using Konfidence.DataBaseInterface;

namespace Konfidence.BaseData.Sp
{
    internal class SpParameter : ISpParameterData
    {
        public string ParameterName { get; set; }

        public DbType DbType { get; set; }

        public object Value { get; set; }

        public SpParameter(string parameterName, DbType dbType, object value)
        {
            ParameterName = parameterName;
            DbType = dbType;
            Value = value;
        }
    }
}
