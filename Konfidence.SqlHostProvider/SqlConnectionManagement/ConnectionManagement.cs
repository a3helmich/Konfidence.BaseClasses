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

        internal static void CopySqlSecurityToClientConfig(IClientConfig clientConfig)
        {
            if (!"ClientConfigLocation".TryGetEnvironmentVariable(out var fileName) || !File.Exists(fileName))
            {
                return;
            }

            var clientSettings = JsonConvert.DeserializeObject<ClientSettings>(File.ReadAllText(fileName));

            if (!clientSettings.DataConfiguration.Connections.Any())
            {
                return;
            }

            foreach (var clientSetting in clientSettings.DataConfiguration.Connections)
            {
                var clientConfigConnections = clientConfig.Connections.Where(x => x.Server == clientSetting.Server);

                foreach (var clientConfigConnection in clientConfigConnections)
                {
                    clientConfigConnection.UserName = clientSetting.UserName;
                    clientConfigConnection.Password = clientSetting.Password;
                }
            }
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

            var connectionString = string.Join(";", connectionStringParts);

            if (connectionStringSettings.ConnectionString == connectionString)
            {
                return config;
            }

            connectionStringSettings.ConnectionString = connectionString;

            ConfigurationManager.RefreshSection("connectionStrings");

            return config;
        }
    }
}
