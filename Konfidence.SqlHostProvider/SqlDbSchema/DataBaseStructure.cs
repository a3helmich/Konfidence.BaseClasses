using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.BaseData;
using Konfidence.DataBaseInterface;
using Konfidence.SqlHostProvider.SqlAccess;
using Ninject;
using Ninject.Parameters;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    [UsedImplicitly]
    public class DatabaseStructure : BaseDataItem
    {
        public TableDataItemList TableList { get; private set; }

        [UsedImplicitly] [NotNull] public string SelectedConnectionName => ConnectionName ?? string.Empty;

        public DatabaseStructure(string connectionName)
        {
            Debug.WriteLine($@"DatabaseStructure constructor{connectionName}");
            ConnectionName = connectionName;
        }

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

            TableList = new TableDataItemList(ConnectionName);

            DeleteStoredProcedures();

            Debug.WriteLine("DatabaseStructure exit BuildStructure()");
        }

        private void CreateStoredProcedures()
        {
            CreateSPPrimaryKey_Get(SpName.PrimarykeyGet);
            CreateSPColumns_GetList(SpName.ColumnsGetlist);
        }

        private void DeleteStoredProcedures()
        {
            DeleteSp(SpName.PrimarykeyGet);
            DeleteSp(SpName.ColumnsGetlist);
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
    }
}
