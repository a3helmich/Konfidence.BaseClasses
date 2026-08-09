namespace Konfidence.SqlHostProvider.SqlConnectionManagement;

/// <summary>
/// Projects the resolved client configuration back onto the host application's configuration, so
/// callers that still read connection details straight from app.config keep seeing what
/// IClientConfig resolved.
/// <para>
/// This is the write half of the legacy System.Configuration coupling. It stays behind an interface
/// because the default implementation rewrites the running application's configuration file on disk
/// and refreshes process-wide sections - a side effect no test can undo cleanly, and one that has
/// nothing to do with the decisions being made about which values to write.
/// </para>
/// </summary>
internal interface IApplicationConfigurationWriter
{
    void SetDefaultDatabase(string connectionName);

    void SetConnectionString(string connectionName, string database, string server);
}
