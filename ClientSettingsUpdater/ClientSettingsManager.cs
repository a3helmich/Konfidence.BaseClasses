using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.SqlHostProvider;
using Konfidence.SqlHostProvider.SqlAccess;
using Newtonsoft.Json;

namespace ClientSettingsUpdater
{
    internal class ClientSettingsManager
    {
        private string _configFolder;
        private string _userName;
        private string _password;

        public ClientSettingsManager([NotNull] string[] args)
        {
            if (!args.Any())
            {
                Environment.Exit(4);
            }

            // location, username, password
            if (!DependencyInjectionFactory.TryProcessArgument(Argument.ConfigFileFolder, args, out _configFolder))
            {
                Environment.Exit(1);
            }

            if (!DependencyInjectionFactory.TryProcessArgument(Argument.UserName, args, out _userName))
            {
                Environment.Exit(2);
            }

            if (!DependencyInjectionFactory.TryProcessArgument(Argument.Password, args, out _password))
            {
                Environment.Exit(3);
            }

        }

        public void Execute()
        {
            if (!Directory.Exists(_configFolder))
            {
                Environment.Exit(5);
            }

            var clientSettingsFileNames = Directory.GetFiles(_configFolder, "clientsettings.json", SearchOption.AllDirectories);

            if (!clientSettingsFileNames.Any())
            {
                Environment.Exit(6);
            }

            foreach (var clientSettingsFileName in clientSettingsFileNames)
            {
                UpdateFile(clientSettingsFileName);
            }
        }

        private void UpdateFile([NotNull] string fileName)
        {
            var clientSettings = JsonConvert.DeserializeObject<ClientSettings>(File.ReadAllText(fileName));

            var connections = clientSettings.DataConfiguration.Connections.Where(x => !x.UserName.IsAssigned());

            clientSettings.DataConfiguration.Connections =
                connections
                    .Select(setting =>
                    {
                        setting.UserName = _userName;
                        setting.Password = _password;

                        return setting;
                    })
                    .ToList();

            File.WriteAllText(fileName, JsonConvert.SerializeObject(clientSettings, Formatting.Indented,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }
    }
}
