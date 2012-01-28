using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.BaseData.Schema;

namespace DataItemGeneratorClasses
{
    public class DataBaseStructure : SchemaBaseDataItem 
    {
        private TableDataItemList _TableList;

        #region readonly properties
        public TableDataItemList TableList
        {
            get { return _TableList; }
        }
        #endregion readonly properties

        public DataBaseStructure()
        {

        }

        public void BuildStructure()
        {
            CreateStoredProcedures();

            //_TableList = new TableDataItemList(_DataBaseName);

            DeleteStoredProcedures();
        }

        private void CreateStoredProcedures()
        {
            if (!StoredProcedureExists("PrimaryKey_Get"))
            {
                CreateSPPrimaryKey_Get();
            }
        }

        private void DeleteStoredProcedures()
        {
            DeleteSP("PrimaryKey_Get");
        }

        private void CreateSPPrimaryKey_Get()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("CREATE PROCEDURE [dbo].[PrimaryKey_Get]");
            sb.AppendLine("  @tableName varchar(50)");
            sb.AppendLine("AS BEGIN");
            sb.AppendLine("  SET NOCOUNT ON;");
            sb.AppendLine("  SELECT 1 as PrimaryKeyId, *");
            sb.AppendLine("  FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS");
            sb.AppendLine("  WHERE constraint_type = 'PRIMARY KEY'");
            sb.AppendLine("    AND table_name = @tableName");
            sb.AppendLine("END");

            string createSPPrimaryKey_Get = sb.ToString();

            ExecuteTextCommand(createSPPrimaryKey_Get);
        }

        private void DeleteSP(string storedProcedure)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("DROP PROCEDURE [dbo].[" + storedProcedure + "]");

            if (StoredProcedureExists(storedProcedure))
            {
                ExecuteTextCommand(sb.ToString());
            }
        }
    }
}
