using System.Data;
using System.Linq;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.BaseData;
using Konfidence.DataBaseInterface;
using Konfidence.SqlHostProvider.SqlAccess;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public class IndexColumnsDataItemProperties : BaseDataItem
    {
        private readonly string _tableName;

        public PrimaryKeyDataItem PrimaryKeyDataItem { get; }

        public IndexColumnsDataItemProperties(string connectionName, string tableName)
        {
            _tableName = tableName;

             ConnectionName = connectionName;

            PrimaryKeyDataItem = new PrimaryKeyDataItem(ConnectionName, _tableName);

            var indexedColumns = Client.GetIndexedColumns();

            GetProperties(indexedColumns);
        }

        [NotNull]
        protected override IBaseClient ClientBind()
        {
            return base.ClientBind<SqlClient>();
        }

        private void GetProperties([NotNull] DataTable dataTable)
        {
            var tableRows = dataTable.AsEnumerable().Where(dataRow => dataRow["TABLE_NAME"].Equals(_tableName));

            var tableRow = tableRows.FirstOrDefault(constraintRow => constraintRow["CONSTRAINT_NAME"].Equals(PrimaryKeyDataItem.ConstraintName));

            if (tableRow.IsAssigned())
            {
                var columnName = tableRow["COLUMN_NAME"] as string;

                PrimaryKeyDataItem.ColumnName = columnName;
            }
        }
    }
}
