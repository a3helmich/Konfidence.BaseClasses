﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.BaseData.Schema;

namespace DataItemGeneratorClasses
{
    public class DataBaseStructure : SchemaBaseDataItem 
    {
        private TableDataItemList _TableList = null;

        #region readonly properties
        public TableDataItemList TableList
        {
            get { return _TableList; }
        }
        #endregion readonly properties

        public DataBaseStructure()
        {
        }

        public DataBaseStructure(string dataBaseName)
        {
            DataBaseName = dataBaseName;
        }

        public void BuildStructure()
        {
            DeleteStoredProcedures(); // cleanup voor als storeprocedures aangepast zijn maar nog niet verwijderd

            CreateStoredProcedures();

            _TableList = new TableDataItemList(DataBaseName);

            DeleteStoredProcedures();
        }

        private void CreateStoredProcedures()
        {
            CreateSPPrimaryKey_Get(SPNames.PRIMARYKEY_GET);
            CreateSPColumns_GetList(SPNames.COLUMNS_GETLIST);
        }

        private void DeleteStoredProcedures()
        {
            DeleteSP(SPNames.PRIMARYKEY_GET);
            DeleteSP(SPNames.COLUMNS_GETLIST);
        }

        private void CreateSPPrimaryKey_Get(string storedProcedure)
        {
            StringBuilder sb = new StringBuilder();

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
            StringBuilder sb = new StringBuilder();

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
