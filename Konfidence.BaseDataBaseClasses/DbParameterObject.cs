using System.Data;

namespace Konfidence.BaseData
{
    public class DbParameterObject
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
