using System;
using System.Collections.Generic;
using System.Data;

namespace Konfidence.BaseData.Schema
{
    public class TableDataItemList: BaseDataItemList<TableDataItem>
    {
        private DataTable _DataTableList;
        private bool _StoredProcedureCreated = false;
        private string _SourcePath;
        private List<string> _GeneratedFileList = null;

        #region properties
        public string SourcePath
        {
            get { return _SourcePath; }
            set { _SourcePath = value; }
        }

        public List<string> GeneratedFileList
        {
            get
            {
                if (!IsAssigned(_GeneratedFileList))
                {
                    _GeneratedFileList = new List<string>();
                }

                BuildGeneratedFileList(_GeneratedFileList);

                return _GeneratedFileList;
            }
        }
        #endregion properties

        private string createStoredProcedureCommand = "CREATE PROCEDURE [dbo].[PrimaryKey_Get] @tableName varchar(50) AS BEGIN SET NOCOUNT ON;  SELECT 1 as PrimaryKeyId, * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE (constraint_type = 'PRIMARY KEY') AND (table_name = @tableName) END";
        private string deleteStoredProcedureCommand = "DROP PROCEDURE [dbo].[PrimaryKey_Get]";

        public TableDataItemList()
        {
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
                ExecuteTextCommand(createStoredProcedureCommand);

                _StoredProcedureCreated = true;
            }
        }

        private void DeleteSchemaCommand()
        {
            if (_StoredProcedureCreated)
            {
                ExecuteTextCommand(deleteStoredProcedureCommand);
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

        private void BuildGeneratedFileList(List<string> generatedFileList)
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
