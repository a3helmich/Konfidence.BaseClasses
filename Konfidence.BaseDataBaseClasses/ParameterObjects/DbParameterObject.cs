using System.Data;
using Konfidence.Base;

namespace Konfidence.BaseData.ParameterObjects
{
    public class DbParameterObject : BaseItem
    {
        public DbParameterObject()
        { }

        public DbParameterObject(string field, DbType dbType, object value)
        {
            Field = field;
            DbType = dbType;
            Value = value;
        }

        #region ParameterObject properties

        public string Field { get; set; }

        public DbType DbType { get; set; }

        public object Value { get; set; }

        #endregion
    }
}
