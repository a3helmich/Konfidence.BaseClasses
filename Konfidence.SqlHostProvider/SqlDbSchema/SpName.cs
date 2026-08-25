using System;

namespace Konfidence.SqlHostProvider.SqlDbSchema;

/// <summary>
/// Names for the helper stored procedures that <see cref="DatabaseStructure"/> creates in the target
/// database, uses, and drops again.
/// <para>
/// The names are unique per instance rather than fixed. Schema introspection is destructive by
/// nature - it drops these procedures when it finishes - so two runs sharing one database used to
/// drop each other's procedures mid-flight, surfacing as "Could not find stored procedure
/// 'CG_Columns_GetList'". Unique names mean a run only ever creates and drops its own.
/// </para>
/// </summary>
internal sealed class SpName
{
    public string GetTablePrimaryKeyList { get; }

    public string GetColumnList { get; }

    public SpName()
    {
        // Process id makes an orphan traceable back to the run that left it; the guid keeps two
        // concurrent runs inside a single process apart.
        string runId = $"{Environment.ProcessId}_{Guid.NewGuid():N}";

        GetTablePrimaryKeyList = $"CG_TableConstraints_GetList_{runId}";
        GetColumnList = $"CG_Columns_GetList_{runId}";
    }
}
