using System.Text;

namespace Konfidence.SqlHostProvider.SqlDbSchema
{
    public class DatabaseStructure : SchemaBaseDataItem
    {
        #region readonly properties
        public TableDataItemList TableList { get; private set; }

        #endregion readonly properties

        //public DatabaseStructure()
        //{
        //}

        public DatabaseStructure(string connectionName)
        {
            ConnectionName = connectionName;
        }

        public void BuildStructure()
        {
            CreateStoredProcedures();

            TableList = new TableDataItemList(ConnectionName);

            DeleteStoredProcedures();
        }

        private void CreateStoredProcedures()
        {
            DeleteStoredProcedures(); // cleanup voor als storeprocedures aangepast zijn maar nog niet verwijderd

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

            ExecuteTextCommand(sb.ToString());
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

            ExecuteTextCommand(sb.ToString());
        }

        private void DeleteSp(string storedProcedure)
        {
            var sb = new StringBuilder();

            sb.AppendLine("DROP PROCEDURE [dbo].[" + storedProcedure + "]");

            if (StoredProcedureExists(storedProcedure))
            {
                ExecuteTextCommand(sb.ToString());
            }
        }
    }
}
