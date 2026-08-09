using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Konfidence.Base;
using Konfidence.SqlDataAccess;

namespace Konfidence.SqlHostProvider.SqlConnectionManagement;

/// <summary>
/// The legacy writer: edits the running application's app.config and refreshes the affected
/// sections. Both operations are no-ops when the corresponding entry is absent, so an application
/// that carries no app.config at all is unaffected.
/// </summary>
internal sealed class AppConfigApplicationConfigurationWriter : IApplicationConfigurationWriter
{
    public void SetDefaultDatabase(string connectionName)
    {
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        DatabaseSettings? databaseSettings = config.Sections[@"dataConfiguration"] as DatabaseSettings;

        if (!databaseSettings.IsAssigned())
        {
            return;
        }

        databaseSettings.DefaultDatabase = connectionName;

        config.Save(ConfigurationSaveMode.Modified);

        ConfigurationManager.RefreshSection("dataConfiguration");
    }

    public void SetConnectionString(string connectionName, string database, string server)
    {
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        ConnectionStringSettings? connectionStringSettings = config.ConnectionStrings
            .ConnectionStrings
            .Cast<ConnectionStringSettings>()
            .FirstOrDefault(x => x.Name == connectionName);

        if (!connectionStringSettings.IsAssigned())
        {
            return;
        }

        List<string> connectionStringParts = connectionStringSettings.ConnectionString.Split([';'], StringSplitOptions.RemoveEmptyEntries).ToList();

        ConnectionManagement.SetConnectionStringPart(connectionStringParts, "Database", database);

        ConnectionManagement.SetConnectionStringPart(connectionStringParts, "Server", server);

        connectionStringSettings.ConnectionString = string.Join(";", connectionStringParts);

        config.Save(ConfigurationSaveMode.Modified);

        ConfigurationManager.RefreshSection("connectionStrings");
    }
}
