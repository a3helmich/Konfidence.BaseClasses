namespace Konfidence.SqlHostProvider.SqlConnectionManagement;

public sealed class ConnectionManager : IConnectionManagement
{
    private readonly IApplicationConfigurationWriter _configurationWriter;

    public ConnectionManager() : this(new AppConfigApplicationConfigurationWriter())
    {
    }

    internal ConnectionManager(IApplicationConfigurationWriter configurationWriter)
    {
        _configurationWriter = configurationWriter;
    }

    public void SetActiveConnection(string connectionName)
    {
        _configurationWriter.SetDefaultDatabase(connectionName);
    }

    public void SetApplicationDatabase(string database, string server, string connectionName)
    {
        _configurationWriter.SetConnectionString(connectionName, database, server);
    }
}
