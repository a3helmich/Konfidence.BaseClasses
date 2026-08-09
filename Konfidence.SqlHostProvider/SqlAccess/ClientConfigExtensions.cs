using System.Linq;
using Konfidence.Base;
using Konfidence.SqlHostProvider.SqlConnectionManagement;

namespace Konfidence.SqlHostProvider.SqlAccess;

internal static class ClientConfigExtensions
{
    public static void SetSqlApplicationSettings(this ClientConfig clientConfig)
    {
        clientConfig.SetSqlApplicationSettings(new AppConfigApplicationConfigurationWriter(), new EnvironmentSqlSecurityFileLocator());
    }

    // The decisions here - whether there is anything to publish, which connection matches, whether
    // credentials still need filling in - are ordinary logic. They only became untestable because
    // they were wired straight to app.config and an environment variable, so both are passed in.
    internal static void SetSqlApplicationSettings(this ClientConfig clientConfig, IApplicationConfigurationWriter configurationWriter, ISqlSecurityFileLocator securityFileLocator)
    {
        if (!clientConfig.DefaultDatabase.IsAssigned())
        {
            return;
        }

        ConfigConnectionString? connection = clientConfig.Connections.FirstOrDefault(x => x.ConnectionName == clientConfig.DefaultDatabase);

        if (connection.IsAssigned())
        {
            configurationWriter.SetConnectionString(connection.ConnectionName, connection.Database, connection.Server);
        }

        configurationWriter.SetDefaultDatabase(clientConfig.DefaultDatabase);

        if (clientConfig.UseEnvironmentSetting && (!connection.IsAssigned() || !connection.UserName.IsAssigned()))
        {
            ConnectionManagement.CopySqlSecurityToClientConfig(clientConfig, securityFileLocator);
        }
    }

    public static ConfigConnectionString? GetConfigConnection(this IClientConfig clientConfig)
    {
        ConfigConnectionString? connection = clientConfig
            .Connections
            .FirstOrDefault(x =>
                clientConfig.DefaultDatabase.IsAssigned() && x.ConnectionName == clientConfig.DefaultDatabase);

        return connection;
    }
}
