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
            var tableRow = dataTable.AsEnumerable().First(dataRow => dataRow["TABLE_NAME"].Equals(_tableName));

            var columnName = tableRow["COLUMN_NAME"] as string;
            var constraintName = tableRow["CONSTRAINT_NAME"] as string;

            if (constraintName.IsAssigned() && constraintName.Equals(PrimaryKeyDataItem.ConstraintName))
            {
                PrimaryKeyDataItem.ColumnName = columnName;
            }
        }
    }
}
