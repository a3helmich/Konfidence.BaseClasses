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
using Newtonsoft.Json;

namespace Konfidence.TestTools
{
    [UsedImplicitly]
    public static class SqlTestToolExtensions
    {
        public static void CopySqlSettingsToActiveConfiguration()
        {
            var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetCallingAssembly().Location);

            var databaseSettings = config.Sections[@"dataConfiguration"] as DatabaseSettings;

            var databaseSettingsCopy = new DatabaseSettings { DefaultDatabase = databaseSettings?.DefaultDatabase };

            var activeConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

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

        public static void CopySqlSecurityToClientConfig(string toConnectionName)
        {
            if (TryGetEnvironmentVariable("ClientConfigLocation", out var fileName) && File.Exists(fileName))
            {
                var clientSettings = JsonConvert.DeserializeObject<ClientSettings>(File.ReadAllText(fileName));

                var connections = clientSettings.DataConfiguration.Connections;

                if (!connections.Any())
                {
                    return;
                }

                var connection = connections.First();

                SetDatabaseSecurity(connection.UserName, connection.Password, toConnectionName);
            }
        }

        //public static void CopySqlSecurityToClientConfig(string fromConnectionName, string toConnectionName)
        //{
        //    if (TryGetEnvironmentVariable("ClientConfigLocation", out var fileName) && File.Exists(fileName))
        //    {
        //        var clientSettings = JsonConvert.DeserializeObject<ClientSettings>(File.ReadAllText(fileName));

        //        var connections = clientSettings
        //            .DataConfiguration
        //            .Connections
        //            .Where(x => x.ConnectionName.Equals(fromConnectionName, StringComparison.OrdinalIgnoreCase))
        //            .ToList();

        //        if (!connections.Any())
        //        {
        //            return;
        //        }

        //        var connection = connections.First();

        //        SetDatabaseSecurity(connection.UserName, connection.Password, toConnectionName);
        //    }
        //}

        private static bool TryGetEnvironmentVariable(string variable, out string value)
        {
            string key;

            var ev = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User);

            if ((key = ev.Keys.Cast<string>().ToList().FirstOrDefault(x => x.Equals(variable, StringComparison.OrdinalIgnoreCase))) != null)
            {
                value = (string) ev[key];

                return true;
            }

            ev = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine);

            if ((key = ev.Keys.Cast<string>().ToList().FirstOrDefault(x => x.Equals(variable, StringComparison.OrdinalIgnoreCase))) != null)
            {
                value = (string)ev[key];

                return true;
            }

            ev = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process);

            if ((key = ev.Keys.Cast<string>().ToList().FirstOrDefault(x => x.Equals(variable, StringComparison.OrdinalIgnoreCase))) != null)
            {
                value = (string)ev[key];

                return true;
            }

            value = string.Empty;

            return false;
        }

        public static void SetDatabaseSecurity(string userName, string password, string connectionName)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (!userName.IsAssigned() || !password.IsAssigned())
            {
                return;
            }

            var connectionStringSettings = config.ConnectionStrings
                .ConnectionStrings
                .Cast<ConnectionStringSettings>()
                .FirstOrDefault(x => x.Name == connectionName);

            if (!connectionStringSettings.IsAssigned())
            {
                return;
            }

            var connectionStringParts = connectionStringSettings.ConnectionString.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).TrimList();

            SetConnectionStringPart(connectionStringParts, "User ID", userName);

            SetConnectionStringPart(connectionStringParts, "Password", password);

            SetConnectionStringPart(connectionStringParts, "Persist Security Info", "True");

            RemoveConnectionStringPart(connectionStringParts, "Integrated Security");

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
    }
}
