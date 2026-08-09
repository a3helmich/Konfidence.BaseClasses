namespace Konfidence.SqlHostProvider.SqlAccess;

/// <summary>
/// Resolves the fallback connection string used when <see cref="IClientConfig"/> holds no matching
/// connection. This exists so the legacy System.Configuration lookup is one swappable implementation
/// rather than a hard-wired static call: IClientConfig is the primary source of truth, and app.config
/// is only consulted when it comes up empty.
/// </summary>
internal interface IDefaultDatabaseProvider
{
    bool TryGetDefaultConnectionString(out string connectionString);
}
