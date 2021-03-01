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
        private readonly string _configFolder;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _server;

        public ClientSettingsManager([NotNull] string[] args)
        {
            if (!args.Any())
            {
                Environment.Exit(4);
            }

            // location, username, password
            if (!args.TryParseArgument(Argument.ConfigFileFolder, out _configFolder))
            {
                Environment.Exit(1);
            }

            if (!args.TryParseArgument(Argument.UserName, out _userName))
            {
                Environment.Exit(2);
            }

            if (!args.TryParseArgument(Argument.Password, out _password))
            {
                Environment.Exit(3);
            }

            if (!args.TryParseArgument(Argument.Server, out _server))
            {
               // not a required field
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

            clientSettings.DataConfiguration.Connections
                .ForEach(setting =>
                {
                    if (setting.UserName.IsAssigned())
                    {
                        return;
                    }

                    if (_server.IsAssigned() && !setting.Server.Equals(_server, StringComparison.OrdinalIgnoreCase))
                    {
                        return;
                    }

                    setting.UserName = _userName;
                    setting.Password = _password;
                });

            File.WriteAllText(fileName, JsonConvert.SerializeObject(clientSettings, Formatting.Indented,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }
    }
}
