using System.Configuration;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;

namespace Konfidence.TestTools
{
    [UsedImplicitly]
    public static class SqlTestToolExtensions
    {
        [UsedImplicitly]
        public static void CopySqlSettingsToActiveConfiguration()
        {
            var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetCallingAssembly().Location);

            var databaseSettings = config.Sections[@"dataConfiguration"] as DatabaseSettings;

            var databaseSettingsCopy = new DatabaseSettings { DefaultDatabase = databaseSettings?.DefaultDatabase };

            var activeConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            activeConfig.Sections.Remove("dataConfiguration");

            activeConfig.Sections.Add("dataConfiguration", databaseSettingsCopy);

            foreach (ConnectionStringSettings configConnectionString in config.ConnectionStrings.ConnectionStrings)
            {
                activeConfig.ConnectionStrings.ConnectionStrings.Remove(configConnectionString);

                activeConfig.ConnectionStrings.ConnectionStrings.Add(configConnectionString);
            }

            activeConfig.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("dataConfiguration");
            ConfigurationManager.RefreshSection("connectionStrings");
        }
    }
}
