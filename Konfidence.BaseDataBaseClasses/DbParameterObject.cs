using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Konfidence.BaseData
{
    public class DbParameterObject
    {
        private string _Field;
        private DbType _DbType;
        private object _Value;

        public DbParameterObject()
        { }

        public DbParameterObject(string field, DbType dbType, object value)
        {
            Field = field;
            DbType = dbType;
            Value = value;
        }

        #region ParameterObject properties
        public string Field
        {
            get { return _Field; }
            set { _Field = value; }
        }

        public DbType DbType
        {
            get { return _DbType; }
            set { _DbType = value; }
        }

        public object Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
        #endregion
    }
}
