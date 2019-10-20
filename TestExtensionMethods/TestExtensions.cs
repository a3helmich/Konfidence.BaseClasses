using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using System.Configuration;
using System.Reflection;

namespace TestExtensionMethods
{
    public static class TestExtensions
    {
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
