using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.BaseData;
using Konfidence.DataBaseInterface;
using Konfidence.SqlHostProvider.SqlAccess;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    [UsedImplicitly]
    public class DatabaseStructure : BaseDataItem
    {
        public List<TableDataItem> TableList { get; private set; }

        [UsedImplicitly] [NotNull] public string SelectedConnectionName => ConnectionName ?? string.Empty;

        public DatabaseStructure(string connectionName)
        {
            Debug.WriteLine($@"DatabaseStructure constructor{connectionName}");
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

            DeleteStoredProcedures(); // cleanup voor als storeprocedures aangepast zijn maar nog niet verwijderd

            Debug.WriteLine("DatabaseStructure between DeleteStoredProcedures() - CreateStoredProcedures()");

            CreateStoredProcedures();

            Debug.WriteLine("DatabaseStructure between CreateStoredProcedures() -  DeleteStoredProcedures()");

            TableList = BuildTableItemList(Client.GetTables());

            DeleteStoredProcedures();

            Debug.WriteLine("DatabaseStructure exit BuildStructure()");
        }

        private void CreateStoredProcedures()
        {
            CreateSPPrimaryKey_Get(SpName.PrimarykeyGet);
            CreateSPColumns_GetList(SpName.GetColumnList);
        }

        private void DeleteStoredProcedures()
        {
            DeleteSp(SpName.PrimarykeyGet);
            DeleteSp(SpName.GetColumnList);
        }

        private void CreateSPPrimaryKey_Get(string storedProcedure)
        {
            var sb = new StringBuilder();

            sb.AppendLine("CREATE PROCEDURE [dbo].[" + storedProcedure + "]");
            sb.AppendLine("  @tableName varchar(50)");
            sb.AppendLine("AS BEGIN");
            sb.AppendLine("  SET NOCOUNT ON;");
            sb.AppendLine("  SELECT 1 as PrimaryKeyId, *");
            sb.AppendLine("  FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS");
            sb.AppendLine("  WHERE constraint_type = 'PRIMARY KEY'");
            sb.AppendLine("    AND table_name = @tableName");
            sb.AppendLine("END");

            Client.ExecuteTextCommand(sb.ToString());
        }

        private void CreateSPColumns_GetList(string storedProcedure)
        {
            var sb = new StringBuilder();

            sb.AppendLine("CREATE PROCEDURE [dbo].[" + storedProcedure + "]");
            sb.AppendLine("@tableName varchar(50)");
            sb.AppendLine("AS BEGIN");
            sb.AppendLine("  SET NOCOUNT ON;");
            sb.AppendLine("  SELECT t.name AS tableName, st.name AS datatype, cc.*");
            sb.AppendLine("  FROM sys.columns cc, sys.tables t, sys.systypes st");
            sb.AppendLine("  WHERE cc.object_id = t.object_id");
            sb.AppendLine("    AND t.name = @tableName");
            sb.AppendLine("    AND st.xtype = cc.system_type_id");
            sb.AppendLine("    AND st.status = 0");
            sb.AppendLine("END");

            Client.ExecuteTextCommand(sb.ToString());
        }

        private void DeleteSp(string storedProcedure)
        {
            Debug.WriteLine($"deleteSp entry");

            var sb = new StringBuilder();

            sb.AppendLine("DROP PROCEDURE [dbo].[" + storedProcedure + "]");

            if (Client.StoredProcedureExists(storedProcedure))
            {
                Client.ExecuteTextCommand(sb.ToString());
            }

            Debug.WriteLine($"deleteSp exit");
        }

        [NotNull]
        private List<TableDataItem> BuildTableItemList([NotNull] DataTable dataTable)
        {
            var allTypes = dataTable.AsEnumerable().ToList();
            var tables = allTypes.Where(dataRow => dataRow["TABLE_TYPE"].Equals("BASE TABLE")).ToList();
            var views = allTypes.Where(dataRow => dataRow["TABLE_TYPE"].Equals("VIEW")).ToList();

            if (allTypes.Count != tables.Count + views.Count)
            {
                throw new Exception("there are unknown tables types");
            }

            var tableList = new List<TableDataItem>();

            foreach (var dataRow in tables)
            {
                var catalog = dataRow["TABLE_CATALOG"] as string;

                var name = dataRow["TABLE_NAME"] as string;

                if (name.IsAssigned() && (!name.Equals("dtproperties", StringComparison.OrdinalIgnoreCase) && !name.StartsWith("sys", StringComparison.OrdinalIgnoreCase)))
                {
                    var tableDataItem = new TableDataItem(ConnectionName, catalog, name);

                    tableList.Add(tableDataItem);
                }
            }

            return tableList;
        }
    }
}
