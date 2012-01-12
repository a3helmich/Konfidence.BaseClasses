using System;
using System.Collections.Generic;
using System.Data;

namespace Konfidence.BaseData.Schema
{
    public class TableDataItemList: BaseDataItemList<TableDataItem>
    {
        private bool _StoredProcedureCreated = false;
        private string _SourcePath;

        private DataTable _DataTableList;
        private List<string> _GeneratedCsFileList = null;
        private List<string> _GeneratedSqlFileList = null;

        #region properties
        public string SourcePath
        {
            get { return _SourcePath; }
            set { _SourcePath = value; }
        }

        public List<string> GeneratedCsFileList
        {
            get
            {
                if (!IsAssigned(_GeneratedCsFileList))
                {
                    _GeneratedCsFileList = new List<string>();
                }

                BuildGeneratedCsFileList(_GeneratedCsFileList);

                return _GeneratedCsFileList;
            }
        }

        public List<string> GeneratedSqlFileList
        {
            get
            {
                if (!IsAssigned(_GeneratedSqlFileList))
                {
                    _GeneratedSqlFileList = new List<string>();
                }

                BuildGeneratedSqlFileList(_GeneratedSqlFileList);

                return _GeneratedSqlFileList;
            }
        }
        #endregion properties

        private string _CreateStoredProcedureCommand = "CREATE PROCEDURE [dbo].[PrimaryKey_Get] @tableName varchar(50) AS BEGIN SET NOCOUNT ON;  SELECT 1 as PrimaryKeyId, * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE (constraint_type = 'PRIMARY KEY') AND (table_name = @tableName) END";
        private string _DeleteStoredProcedureCommand = "DROP PROCEDURE [dbo].[PrimaryKey_Get]";

        public TableDataItemList(string dataBaseName)
        {
            DataBaseName = dataBaseName;

            CreateSchemaCommand();

            BuildItemList(_DataTableList);

            DeleteSchemaCommand();
        }

        protected override void InitializeDataItemList()
        {
            TableDataItem tableDataItem = GetNewDataItem();

            _DataTableList = tableDataItem.GetSchemaObject("Tables");
        }

        private void CreateSchemaCommand()
        {
            if (!StoredProcedureExists("PrimaryKey_Get"))
            {
                ExecuteTextCommand(_CreateStoredProcedureCommand);

                _StoredProcedureCreated = true;
            }
        }

        private void DeleteSchemaCommand()
        {
            if (_StoredProcedureCreated)
            {
                ExecuteTextCommand(_DeleteStoredProcedureCommand);
            }
        }

        private void BuildItemList(DataTable dataTableList)
        {
            foreach (DataRow dataRow in dataTableList.Rows)
            {
                string tableType = dataRow["TABLE_TYPE"] as string;

                switch (tableType)
                {
                    case "BASE TABLE":
                        TableDataItem tableDataItem = null;

                        string catalog = dataRow["TABLE_CATALOG"] as string;
                        string schema = dataRow["TABLE_SCHEMA"] as string;
                        string name = dataRow["TABLE_NAME"] as string;

                        if (!name.ToLower().Equals("dtproperties"))
                        {
                            tableDataItem = new TableDataItem(catalog, schema, name);

                            this.Add(tableDataItem);
                        }

                        break;
                    case "VIEW":
                        break;
                    default:
                        throw new Exception("onbekend tabletype");
                }
            }
        }

        private void BuildGeneratedCsFileList(List<string> generatedFileList)
        {
            generatedFileList.Clear();

            foreach (TableDataItem tableDataItem in this)
            {
                foreach (string fileName in tableDataItem.GeneratedCsFileList)
                {
                    if (!generatedFileList.Contains(fileName))
                    {
                        generatedFileList.Add(fileName);
                    }
                }
            }
        }

        private void BuildGeneratedSqlFileList(List<string> generatedFileList)
        {
            generatedFileList.Clear();

            foreach (TableDataItem tableDataItem in this)
            {
                foreach (string fileName in tableDataItem.GeneratedSqlFiles)
                {
                    if (!generatedFileList.Contains(fileName))
                    {
                        generatedFileList.Add(fileName);
                    }
                }
            }
        }

        protected override TableDataItem GetNewDataItem()
        {
            return new TableDataItem();
        }
    }
}
