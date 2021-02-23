using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Konfidence.Base;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;

namespace Konfidence.SqlHostProvider.SqlConnectionManagement
{
    public class ConnectionManagement
    {
        [UsedImplicitly]
        public static void SetActiveConnection(string connectionName)
        {
            var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetCallingAssembly().Location);

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
        public static void SetApplicationDatabase(string dataBase, string server, string connectionName)
        {
            var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetCallingAssembly().Location);

            var connectionStringSettings = config.ConnectionStrings
                .ConnectionStrings
                .Cast<ConnectionStringSettings>()
                .FirstOrDefault(x => x.Name == connectionName);

            if (!connectionStringSettings.IsAssigned())
            {
                return;
            }

            var connectionStringParts = connectionStringSettings.ConnectionString.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries).TrimList();

            connectionStringParts[0] = $"Database={dataBase}";

            connectionStringParts[1] = $"Server={server}";

            connectionStringSettings.ConnectionString = string.Join(";", connectionStringParts);

            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("connectionStrings");
        }
    }
}
