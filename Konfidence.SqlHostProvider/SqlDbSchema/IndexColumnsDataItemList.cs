﻿using System.Data;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.BaseData;
using Konfidence.DataBaseInterface;
using Konfidence.SqlHostProvider.SqlAccess;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public class IndexColumnsDataItemList : BaseDataItemList<IndexColumnsDataItem>
    {
        private readonly string _tableName;

        public PrimaryKeyDataItem PrimaryKeyDataItem { get; }

        public IndexColumnsDataItemList(string connectionName, string tableName)
        {
            _tableName = tableName;

             ConnectionName = connectionName;

            PrimaryKeyDataItem = new PrimaryKeyDataItem(ConnectionName, _tableName);

            var indexColumnDataItem = new IndexColumnsDataItem(ConnectionName);

            var dataTable = indexColumnDataItem.GetIndexedColumns();

            BuildItemList(dataTable);
        }

        [NotNull]
        protected override IBaseClient ClientBind()
        {
            return base.ClientBind<SqlClient>();
        }

        private void BuildItemList([NotNull] DataTable dataTable)
        {
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var tableName = dataRow["TABLE_NAME"] as string;

                if (tableName.IsAssigned() && tableName.Equals(_tableName))
                {
                    var columnName = dataRow["COLUMN_NAME"] as string;
                    var constraintName = dataRow["CONSTRAINT_NAME"] as string;

                    var indexColumnDataItem = new IndexColumnsDataItem(ConnectionName);

                    if (constraintName.IsAssigned() && constraintName.Equals(PrimaryKeyDataItem.ConstraintName))
                    {
                        PrimaryKeyDataItem.ColumnName = columnName;
                    }

                    Add(indexColumnDataItem);
                }
            }
        }
    }
}
