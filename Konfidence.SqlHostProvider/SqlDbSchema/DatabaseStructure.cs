using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using JetBrains.Annotations;
using Konfidence.DatabaseInterface;

namespace Konfidence.SqlHostProvider.SqlDbSchema;

[UsedImplicitly]
internal class DatabaseStructure : IDatabaseStructure
{
    public List<ITableDataItem> Tables { get; }

    private readonly IBaseClient _client;

    private readonly List<IColumnDataItem> _allColumnDataItems;

    private readonly List<IPrimaryKeyDataItem> _allPrimaryKeyDataItems;

    private readonly List<IIndexDataItem> _allIndexDataItems;

    private readonly SpName _spName = new();

    public DatabaseStructure(IBaseClient client)
    {
        _client = client;

        Tables = new List<ITableDataItem>();

        _allColumnDataItems = new List<IColumnDataItem>();
        _allPrimaryKeyDataItems = new List<IPrimaryKeyDataItem>();
        _allIndexDataItems = new List<IIndexDataItem>();
    }

    [UsedImplicitly]
    public void BuildStructure()
    {
        Debug.WriteLine("DatabaseStructure enter BuildStructure()");

        Initialize();

        // Everything from the first CREATE onwards runs under a finally, so the helper procedures
        // are dropped even when introspection throws part-way through - otherwise a failed run
        // leaves them behind in the caller's database.
        try
        {
            CreateStoredProcedures();

            _allPrimaryKeyDataItems.AddRange(PrimaryKeyDataItem.GetList(_client, _spName.GetTablePrimaryKeyList));

            _allIndexDataItems.AddRange(IndexDataItem.GetList(_client, _allPrimaryKeyDataItems));

            _allColumnDataItems.AddRange(ColumnDataItem.GetList(_client, _allIndexDataItems, _spName.GetColumnList));

            Tables.AddRange(TableDataItem.GetList(_client, _allColumnDataItems));
        }
        finally
        {
            DeleteStoredProcedures();
        }

        Debug.WriteLine("DatabaseStructure exit BuildStructure()");
    }

    private void Initialize()
    {
        Tables.Clear();

        _allPrimaryKeyDataItems.Clear();
        _allIndexDataItems.Clear();
        _allColumnDataItems.Clear();
    }

    private void CreateStoredProcedures()
    {
        CreateSPTablePrimaryKey_GetList(_spName.GetTablePrimaryKeyList);
        CreateSPColumns_GetList(_spName.GetColumnList);
    }

    private void DeleteStoredProcedures()
    {
        // Each drop stands on its own: a failure dropping the first must not leave the second
        // behind.
        DeleteSpIgnoringFailure(_spName.GetTablePrimaryKeyList);
        DeleteSpIgnoringFailure(_spName.GetColumnList);
    }

    private void DeleteSpIgnoringFailure(string storedProcedure)
    {
        try
        {
            DeleteSp(storedProcedure);
        }
        catch (Exception exception)
        {
            Debug.WriteLine($"deleteSp failed for {storedProcedure}: {exception}");
        }
    }

    private void CreateSPTablePrimaryKey_GetList(string storedProcedure)
    {
        StringBuilder sb = new();

        sb.AppendLine($"CREATE PROCEDURE [dbo].[{storedProcedure}]");
        sb.AppendLine("AS BEGIN");
        sb.AppendLine("  SET NOCOUNT ON;");
        sb.AppendLine("  SELECT 1 as PrimaryKeyId, *");
        sb.AppendLine("  FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS");
        sb.AppendLine("  WHERE constraint_type = 'PRIMARY KEY'");
        sb.AppendLine("END");

        _client.ExecuteTextCommand(sb.ToString());
    }

    private void CreateSPColumns_GetList(string storedProcedure)
    {
        StringBuilder sb = new();

        sb.AppendLine($"CREATE PROCEDURE [dbo].[{storedProcedure}]");
        sb.AppendLine("AS BEGIN");
        sb.AppendLine("  SET NOCOUNT ON;");
        sb.AppendLine("  SELECT t.name AS tableName, st.name AS datatype, cc.*");
        sb.AppendLine("  FROM sys.columns cc, sys.tables t, sys.systypes st");
        sb.AppendLine("  WHERE cc.object_id = t.object_id");
        sb.AppendLine("    AND st.xtype = cc.system_type_id");
        sb.AppendLine("    AND st.status = 0");
        sb.AppendLine("END");

        _client.ExecuteTextCommand(sb.ToString());
    }

    private void DeleteSp(string storedProcedure)
    {
        Debug.WriteLine("deleteSp entry");

        StringBuilder sb = new();

        sb.AppendLine($"DROP PROCEDURE [dbo].[{storedProcedure}]");

        if (_client.StoredProcedureExists(storedProcedure))
        {
            _client.ExecuteTextCommand(sb.ToString());
        }

        Debug.WriteLine("deleteSp exit");
    }
}
