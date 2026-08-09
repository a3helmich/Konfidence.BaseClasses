using System.Configuration;
using Konfidence.Base;
using Konfidence.SqlDataAccess;

namespace Konfidence.SqlHostProvider.SqlAccess;

/// <summary>
/// The legacy fallback: reads the default database name from the app.config "dataConfiguration"
/// section and resolves it against &lt;connectionStrings&gt;. This is the last remaining consumer of
/// System.Configuration in the read path, kept behind <see cref="IDefaultDatabaseProvider"/> so
/// applications configured entirely through IClientConfig never touch it.
/// </summary>
internal sealed class AppConfigDefaultDatabaseProvider : IDefaultDatabaseProvider
{
    public bool TryGetDefaultConnectionString(out string connectionString)
    {
        connectionString = string.Empty;

        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        DatabaseSettings? databaseSettings = config.Sections["dataConfiguration"] as DatabaseSettings;

        string? defaultDatabaseName = databaseSettings?.DefaultDatabase;

        if (!defaultDatabaseName.IsAssigned())
        {
            return false;
        }

        ConnectionStringSettings? connectionStringSettings = config.ConnectionStrings.ConnectionStrings[defaultDatabaseName];

        if (!connectionStringSettings.IsAssigned())
        {
            return false;
        }

        connectionString = connectionStringSettings.ConnectionString;

        return true;
    }
}
