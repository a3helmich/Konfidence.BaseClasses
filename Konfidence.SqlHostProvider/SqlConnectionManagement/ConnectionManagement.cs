using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.SqlDataAccess;
using Konfidence.SqlHostProvider.SqlAccess;

namespace Konfidence.SqlHostProvider.SqlConnectionManagement
{
    public class ConnectionManagement
    {
        [UsedImplicitly]
        public static void SetActiveConnection(string connectionName)
        {
            Configuration? config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            DatabaseSettings? databaseSettings = config.Sections[@"dataConfiguration"] as DatabaseSettings;

            if (!databaseSettings.IsAssigned())
            {
                return;
            }

            databaseSettings.DefaultDatabase = connectionName;

            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("dataConfiguration");
        }

        [UsedImplicitly]
        public static void SetApplicationDatabase(string database, string server, string connectionName)
        {
            Configuration? config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            ConnectionStringSettings? connectionStringSettings = config.ConnectionStrings
                .ConnectionStrings
                .Cast<ConnectionStringSettings>()
                .FirstOrDefault(x => x.Name == connectionName);

            if (!connectionStringSettings.IsAssigned())
            {
                return;
            }

            List<string> connectionStringParts = connectionStringSettings.ConnectionString.Split([';'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.RemoveEmptyEntries).ToList();

            SetConnectionStringPart(connectionStringParts, "Database", database);

            SetConnectionStringPart(connectionStringParts, "Server", server);

            connectionStringSettings.ConnectionString = string.Join(";", connectionStringParts);

            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("connectionStrings");
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
            if (!"ClientConfigLocation".TryGetEnvironmentVariable(out string fileName) || !File.Exists(fileName))
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
}
