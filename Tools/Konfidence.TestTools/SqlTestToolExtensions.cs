using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Linq;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.SqlHostProvider.SqlAccess;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using System.Text.Json;

namespace Konfidence.TestTools
{
    [UsedImplicitly]
    public static class SqlTestToolExtensions
    {
        public static void CopySqlSettingsToActiveConfiguration()
        {
            Configuration? config = ConfigurationManager.OpenExeConfiguration(Assembly.GetCallingAssembly().Location);

            DatabaseSettings? databaseSettings = config.Sections[@"dataConfiguration"] as DatabaseSettings;

            DatabaseSettings databaseSettingsCopy = new() { DefaultDatabase = databaseSettings?.DefaultDatabase };

            Configuration? activeConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            activeConfig.Sections.Remove("dataConfiguration");

            activeConfig.Sections.Add("dataConfiguration", databaseSettingsCopy);

            activeConfig.ConnectionStrings.ConnectionStrings.Clear();

            foreach (ConnectionStringSettings configConnectionStringSettings in config.ConnectionStrings.ConnectionStrings)
            {
                activeConfig.ConnectionStrings.ConnectionStrings.Add(configConnectionStringSettings);
            }

            activeConfig.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("dataConfiguration");
            ConfigurationManager.RefreshSection("connectionStrings");
        }

        public static void CopySqlSecurityToActiveConfiguration(string connectionName)
        {
            if ("ClientConfigLocation".TryGetEnvironmentVariable(out string? fileName) && File.Exists(fileName))
            {
                ClientSettings? clientSettings = JsonSerializer.Deserialize<ClientSettings>(File.ReadAllText(fileName));

                List<ConfigConnectionString>? connections = clientSettings?.DataConfiguration?.Connections;

                if (!connections.IsAssigned() || !connections.Any())
                {
                    return;
                }

                ConfigConnectionString? connection = connections.FirstOrDefault(c => c.ConnectionName.Equals(connectionName, StringComparison.OrdinalIgnoreCase));

                if (!connection.IsAssigned())
                {
                    connection = connections.First();
                }

                SaveDatabaseSecurityToActiveConfiguration(connection.UserName, connection.Password, connectionName);
            }
        }

        private static void SaveDatabaseSecurityToActiveConfiguration(string userName, string password, string connectionName)
        {
            Configuration? config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (!userName.IsAssigned() || !password.IsAssigned())
            {
                return;
            }

            ConnectionStringSettings? connectionStringSettings = config.ConnectionStrings
                .ConnectionStrings
                .Cast<ConnectionStringSettings>()
                .FirstOrDefault(x => x.Name == connectionName);

            if (!connectionStringSettings.IsAssigned())
            {
                return;
            }

            List<string> connectionStringParts = connectionStringSettings.ConnectionString.Split([ ';' ], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

            SetConnectionStringPart(connectionStringParts, "User ID", userName);

            SetConnectionStringPart(connectionStringParts, "Password", password);

            SetConnectionStringPart(connectionStringParts, "Persist Security Info", "True");

            RemoveConnectionStringPart(connectionStringParts, "Integrated Security");

            connectionStringSettings.ConnectionString = string.Join(";" , connectionStringParts);

            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("connectionStrings");
        }

        private static void SetConnectionStringPart(List<string> connectionStringParts, string parameter, string value)
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

        private static void RemoveConnectionStringPart(List<string> connectionStringParts, string parameter)
        {
            string connectionPart = connectionStringParts
                .FirstOrDefault(x =>
                    x.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) &&
                    x.TrimStartIgnoreCase(parameter).StartsWith("=")) ?? string.Empty;

            connectionStringParts.Remove(connectionPart);
        }
    }
}
