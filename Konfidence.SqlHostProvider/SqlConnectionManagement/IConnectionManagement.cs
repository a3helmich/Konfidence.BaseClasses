namespace Konfidence.SqlHostProvider.SqlConnectionManagement;

/// <summary>
/// Points the named connections declared in the host application's configuration at a database and
/// server, and selects which of them is the active one.
/// <para>
/// Both operations act on a connection <em>name</em>, so an application that works against more than
/// one target - a development and a deployment database, say - declares a name per target and
/// switches between them. A name that the host configuration does not declare is ignored.
/// </para>
/// </summary>
public interface IConnectionManagement
{
    void SetActiveConnection(string connectionName);

    void SetApplicationDatabase(string database, string server, string connectionName);
}
