using System;
using System.Collections.Generic;
using System.Text;

namespace Konfidence.BaseData.Schema
{
    public class IndexColumnsDataItem: SchemaBaseDataItem
    {
        private string _ColumnName = string.Empty;
        private string _ConstraintName = string.Empty;

        public IndexColumnsDataItem()
        {
            //SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTSWHERE (constraint_type = 'PRIMARY KEY') AND (table_name = @tableName)
        }

        public IndexColumnsDataItem(string columnName, string constraintName) : this()
        {
            _ColumnName = columnName;
            _ConstraintName = constraintName;
        }
    }
}
