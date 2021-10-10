using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.Mail;
using Konfidence.SqlHostProvider;
using Konfidence.SqlHostProvider.SqlAccess;
using Konfidence.SqlHostProvider.SqlConnectionManagement;
using Newtonsoft.Json;

namespace ClientSettingsUpdater
{
    internal class ClientSettingsManager
    {
        public readonly string ConfigFolder;
        public readonly string UserName;
        public readonly string Password;
        public readonly string Server;
        public readonly string ConfigFileName;
        public readonly string MailServer;

        public ClientSettingsManager([NotNull] string[] args, IErrorExiter errorExiter)
        {
            if (!args.Any())
            {
                errorExiter.Exit(4);

                return;
            }

            // location, username, password
            if (!args.TryParseArgument(Argument.ConfigFileFolder, out ConfigFolder))
            {
                errorExiter.Exit(1);

                return;
            }

            if (!args.TryParseArgument(Argument.UserName, out UserName))
            {
                errorExiter.Exit(2);

                return;
            }

            if (!args.TryParseArgument(Argument.Password, out Password))
            {
                errorExiter.Exit(3);

                return;
            }

            if (!args.TryParseArgument(Argument.Server, out Server))
            {
                Server = string.Empty; // not required
            }

            if (!args.TryParseArgument(Argument.MailServer, out MailServer))
            {
                MailServer = string.Empty; // not required
            }

            if (!args.TryParseArgument(Argument.ConfigFileName, out ConfigFileName))
            {
                ConfigFileName = SqlConnectionConstants.DefaultConfigFileName;

                if (MailServer.IsAssigned())
                {
                    ConfigFileName = MailConstants.DefaultMailServerConfigFileName;
                }
            }
        }

        public void Execute()
        {
            if (!Directory.Exists(ConfigFolder))
            {
                Environment.Exit(6);
            }

            var clientSettingsFileNames = Directory.GetFiles(ConfigFolder, ConfigFileName, SearchOption.AllDirectories);

            if (!clientSettingsFileNames.Any())
            {
                Environment.Exit(7);
            }

            var fullFolderName = Path.GetFullPath(ConfigFolder);

            Debug.WriteLine($"Location: {fullFolderName}");
            Console.WriteLine($"Location: {fullFolderName}");

            foreach (var clientSettingsFileName in clientSettingsFileNames)
            {
                Debug.WriteLine($"File: {clientSettingsFileName}");
                Console.WriteLine($"File: {clientSettingsFileName}");

                if (MailServer.IsAssigned())
                {
                    UpdateMailServerFile(clientSettingsFileName);

                    continue;
                }

                UpdateFile(clientSettingsFileName);
            }
        }

        private void UpdateMailServerFile([NotNull] string fileName)
        {
            var clientSettings = JsonConvert.DeserializeObject<MailAccounts>(File.ReadAllText(fileName));

            clientSettings.Accounts
                .ForEach(setting =>
                {
                    if (setting.UserName.IsAssigned())
                    {
                        return;
                    }

                    if (MailServer.IsAssigned() && !setting.Server.Equals(MailServer, StringComparison.OrdinalIgnoreCase))
                    {
                        return;
                    }

                    setting.UserName = UserName;
                    setting.Password = Password;
                });

            if (clientSettings.Accounts.All(x => x.UserName != UserName))
            {
                var account = new MailAccount
                {
                    Server = MailServer,
                    UserName = UserName,
                    Password = Password
                };

                clientSettings.Accounts.Add(account);
            }

            File.WriteAllText(fileName, JsonConvert.SerializeObject(clientSettings, Formatting.Indented,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
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

                    if (Server.IsAssigned() && !setting.Server.Equals(Server, StringComparison.OrdinalIgnoreCase))
                    {
                        return;
                    }

                    setting.UserName = UserName;
                    setting.Password = Password;
                });

            File.WriteAllText(fileName, JsonConvert.SerializeObject(clientSettings, Formatting.Indented,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }
    }
}
