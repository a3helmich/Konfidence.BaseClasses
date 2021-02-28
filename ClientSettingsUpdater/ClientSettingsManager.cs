using System;
using System.Diagnostics;
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
        private string _server = string.Empty;

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

            if (!DependencyInjectionFactory.TryProcessArgument(Argument.Server, args, out _server))
            {
               // not required
            }
        }

        public void Execute()
        {
            if (!Directory.Exists(_configFolder))
            {
                Environment.Exit(6);
            }

            var clientSettingsFileNames = Directory.GetFiles(_configFolder, "clientsettings.json", SearchOption.AllDirectories);

            if (!clientSettingsFileNames.Any())
            {
                Environment.Exit(7);
            }

            var fullFolderName = Path.GetFullPath(_configFolder);

            Debug.WriteLine($"Location: {fullFolderName}");
            Console.WriteLine($"Location: {fullFolderName}");

            foreach (var clientSettingsFileName in clientSettingsFileNames)
            {
                Debug.WriteLine($"File: {clientSettingsFileName}");
                Console.WriteLine($"File: {clientSettingsFileName}");

                UpdateFile(clientSettingsFileName);
            }
        }

        private void UpdateFile([NotNull] string fileName)
        {
            var clientSettings = JsonConvert.DeserializeObject<ClientSettings>(File.ReadAllText(fileName));

            var connections = clientSettings.DataConfiguration.Connections;

            clientSettings.DataConfiguration.Connections = connections
                .Select(setting =>
                {
                    if (setting.UserName.IsAssigned())
                    {
                        return setting;
                    }

                    if (setting.Server.Equals(_server, StringComparison.OrdinalIgnoreCase))
                    {
                        setting.UserName = _userName;
                        setting.Password = _password;

                        return setting;
                    }

                    if (_server.IsAssigned())
                    {
                        return setting;
                    }

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
