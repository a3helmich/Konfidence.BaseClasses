using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.SqlDataAccess;
using Konfidence.SqlHostProvider.SqlAccess;

namespace Konfidence.SqlHostProvider.SqlConnectionManagement;

public class ConnectionManagement
{
    [UsedImplicitly]
    public static void SetActiveConnection(string connectionName)
    {
        new ConnectionManager().SetActiveConnection(connectionName);
    }

    [UsedImplicitly]
    public static void SetApplicationDatabase(string database, string server, string connectionName)
    {
        new ConnectionManager().SetApplicationDatabase(database, server, connectionName);
    }

    internal static void SetConnectionStringPart(List<string> connectionStringParts, string parameter, string value)
    {
        if (!value.IsAssigned())
        {
            return;
        }

        string connectionPart = connectionStringParts
            .FirstOrDefault(x =>
                x.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) &&
                x.TrimStartIgnoreCase(parameter).StartsWith("=")) ?? string.Empty;

        connectionStringParts.Remove(connectionPart);

        connectionStringParts.Add($"{parameter}={value}");
    }

    internal static void CopySqlSecurityToClientConfig(IClientConfig clientConfig)
    {
        CopySqlSecurityToClientConfig(clientConfig, new EnvironmentSqlSecurityFileLocator());
    }

    internal static void CopySqlSecurityToClientConfig(IClientConfig clientConfig, ISqlSecurityFileLocator securityFileLocator)
    {
        if (!securityFileLocator.TryGetSecurityFilePath(out string fileName))
        {
            return;
        }

        CopySqlSecurityToClientConfig(clientConfig, fileName);
    }

    // Kept separate from the locator above so the file handling can be exercised against a fixture
    // path: the default locator reads an environment variable that TryGetEnvironmentVariable
    // resolves User scope first, so a test process can neither redirect nor clear it.
    internal static void CopySqlSecurityToClientConfig(IClientConfig clientConfig, string fileName)
    {
        if (!File.Exists(fileName))
        {
            return;
        }

        if (!File.ReadAllText(fileName).Deserialize(out ClientSettings? clientSettings) || !clientSettings.DataConfiguration.IsAssigned() || !clientSettings.DataConfiguration.Connections.Any())
        {
            return;
        }

        foreach (ConfigConnectionString clientSetting in clientSettings.DataConfiguration.Connections)
        {
            IEnumerable<ConfigConnectionString> clientConfigConnections = clientConfig.Connections.Where(x => x.Server == clientSetting.Server);

            foreach (ConfigConnectionString clientConfigConnection in clientConfigConnections)
            {
                clientConfigConnection.UserName = clientSetting.UserName;
                clientConfigConnection.Password = clientSetting.Password;
            }
        }
    }
}
