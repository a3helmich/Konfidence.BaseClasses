using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using JetBrains.Annotations;
using Konfidence.BaseData;
using Konfidence.DataBaseInterface;
using Konfidence.SqlHostProvider.SqlAccess;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    [UsedImplicitly]
    public class DatabaseStructure : BaseDataItem
    {
        public List<ITableDataItem> Tables { get; }

        [UsedImplicitly] [NotNull] public string SelectedConnectionName => ConnectionName ?? string.Empty;

        private readonly List<IColumnDataItem> _allColumnDataItems;

        private readonly List<IPrimaryKeyDataItem> _allPrimaryKeyDataItems;

        private readonly List<IIndexDataItem> _allIndexDataItems;

        public DatabaseStructure(string connectionName)
        {
            Tables = new List<ITableDataItem>();

            _allColumnDataItems = new List<IColumnDataItem>();
            _allPrimaryKeyDataItems = new List<IPrimaryKeyDataItem>();
            _allIndexDataItems = new List<IIndexDataItem>();

            Debug.WriteLine($"DatabaseStructure constructor{connectionName}");
            ConnectionName = connectionName;
        }

        [NotNull]
        protected override IBaseClient ClientBind()
        {
            return base.ClientBind<SqlClient>();
        }

        [UsedImplicitly]
        public void BuildStructure()
        {
            Debug.WriteLine("DatabaseStructure enter BuildStructure()");

            DeleteStoredProcedures();

            Debug.WriteLine("DatabaseStructure between DeleteStoredProcedures() - CreateStoredProcedures()");

            CreateStoredProcedures();

            Debug.WriteLine("DatabaseStructure between CreateStoredProcedures() -  DeleteStoredProcedures()");

            _allPrimaryKeyDataItems.AddRange(PrimaryKeyDataItem.GetList(Client));

            _allIndexDataItems.AddRange(IndexDataItem.GetList(Client, _allPrimaryKeyDataItems));
            
            _allColumnDataItems.AddRange(ColumnDataItem.GetList(Client, _allIndexDataItems));

            Tables.AddRange(TableDataItem.GetList(Client, _allColumnDataItems));

            DeleteStoredProcedures();

            Debug.WriteLine("DatabaseStructure exit BuildStructure()");
        }

        private void CreateStoredProcedures()
        {
            CreateSPTablePrimaryKey_GetList(SpName.GetTablePrimaryKeyList);
            CreateSPColumns_GetList(SpName.GetColumnList);
        }

        private void DeleteStoredProcedures()
        {
            DeleteSp(SpName.GetTablePrimaryKeyList);
            DeleteSp(SpName.GetColumnList);
        }

        private void CreateSPTablePrimaryKey_GetList(string storedProcedure)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"CREATE PROCEDURE [dbo].[{storedProcedure}]");
            sb.AppendLine("AS BEGIN");
            sb.AppendLine("  SET NOCOUNT ON;");
            sb.AppendLine("  SELECT 1 as PrimaryKeyId, *");
            sb.AppendLine("  FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS");
            sb.AppendLine("  WHERE constraint_type = 'PRIMARY KEY'");
            sb.AppendLine("END");

            Client.ExecuteTextCommand(sb.ToString());
        }

        private void CreateSPColumns_GetList(string storedProcedure)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"CREATE PROCEDURE [dbo].[{storedProcedure}]");
            sb.AppendLine("AS BEGIN");
            sb.AppendLine("  SET NOCOUNT ON;");
            sb.AppendLine("  SELECT t.name AS tableName, st.name AS datatype, cc.*");
            sb.AppendLine("  FROM sys.columns cc, sys.tables t, sys.systypes st");
            sb.AppendLine("  WHERE cc.object_id = t.object_id");
            sb.AppendLine("    AND st.xtype = cc.system_type_id");
            sb.AppendLine("    AND st.status = 0");
            sb.AppendLine("END");

            Client.ExecuteTextCommand(sb.ToString());
        }

        private void DeleteSp(string storedProcedure)
        {
            Debug.WriteLine("deleteSp entry");

            var sb = new StringBuilder();

            sb.AppendLine($"DROP PROCEDURE [dbo].[{storedProcedure}]");

            if (Client.StoredProcedureExists(storedProcedure))
            {
                Client.ExecuteTextCommand(sb.ToString());
            }

            Debug.WriteLine("deleteSp exit");
        }
    }
}
