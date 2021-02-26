using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.SqlHostProvider.SqlAccess;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Newtonsoft.Json;

namespace Konfidence.SqlHostProvider.SqlConnectionManagement
{
    public class ConnectionManagement
    {
        [UsedImplicitly]
        public static void SetActiveConnection(string connectionName)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            var databaseSettings = config.Sections[@"dataConfiguration"] as DatabaseSettings;

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
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            var connectionStringSettings = config.ConnectionStrings
                .ConnectionStrings
                .Cast<ConnectionStringSettings>()
                .FirstOrDefault(x => x.Name == connectionName);

            if (!connectionStringSettings.IsAssigned())
            {
                return;
            }

            var connectionStringParts = connectionStringSettings.ConnectionString.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries).TrimList();

            SetConnectionStringPart(connectionStringParts, "Database", database);

            SetConnectionStringPart(connectionStringParts, "Server", server);

            connectionStringSettings.ConnectionString = string.Join(";", connectionStringParts);

            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("connectionStrings");
        }

        private static void SetConnectionStringPart([NotNull] List<string> connectionStringParts, string parameter, string value)
        {
            if (!value.IsAssigned())
            {
                return;
            }

            var connectionPart = connectionStringParts
                .FirstOrDefault(x => x.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) && x.TrimStartIgnoreCase(parameter).StartsWith("="));

            connectionStringParts.Remove(connectionPart);

            connectionStringParts.Add($"{parameter}={value}");
        }

        private static void RemoveConnectionStringPart([NotNull] List<string> connectionStringParts, string parameter)
        {
            var connectionPart = connectionStringParts
                .FirstOrDefault(x => x.StartsWith(parameter, StringComparison.OrdinalIgnoreCase) && x.TrimStartIgnoreCase(parameter).StartsWith("="));

            connectionStringParts.Remove(connectionPart);
        }

        internal static void CopySqlSecurityToMemory(string connectionName)
        {
            if (!"ClientConfigLocation".TryGetEnvironmentVariable(out var fileName) || !File.Exists(fileName))
            {
                return;
            }

            var clientSettings = JsonConvert.DeserializeObject<ClientSettings>(File.ReadAllText(fileName));

            var connections = clientSettings
                .DataConfiguration
                .Connections
                .Where(x => x.ConnectionName.Equals(connectionName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!connections.Any())
            {
                return;
            }

            var connection = connections.First();

            SetDatabaseSecurityInMemory(connection.UserName, connection.Password, connection.ConnectionName);
        }

        [NotNull]
        internal static Configuration SetDatabaseSecurityInMemory(string userName, string password, string connectionName)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            var connectionStringSettings = config.ConnectionStrings
                .ConnectionStrings
                .Cast<ConnectionStringSettings>()
                .FirstOrDefault(x => x.Name == connectionName);

            if (!userName.IsAssigned() || !password.IsAssigned() || !connectionStringSettings.IsAssigned())
            {
                return config;
            }

            var connectionStringParts = connectionStringSettings.ConnectionString.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).TrimList();

            SetConnectionStringPart(connectionStringParts, "User ID", userName);

            SetConnectionStringPart(connectionStringParts, "Password", password);

            SetConnectionStringPart(connectionStringParts, "Persist Security Info", "True");

            RemoveConnectionStringPart(connectionStringParts, "Integrated Security");

            connectionStringSettings.ConnectionString = string.Join(";", connectionStringParts);

            ConfigurationManager.RefreshSection("connectionStrings");

            return config;
        }
    }
}
