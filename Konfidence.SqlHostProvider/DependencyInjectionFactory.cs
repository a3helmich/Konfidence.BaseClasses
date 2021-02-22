using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.DataBaseInterface;
using Konfidence.SqlHostProvider.SqlAccess;
using Konfidence.SqlHostProvider.SqlDbSchema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Konfidence.SqlHostProvider
{
    public class DependencyInjectionFactory
    {
        private static IConfigurationRoot GetConfigurationRoot(string[] args)
        {
            return new ConfigurationBuilder()
                .SetBasePath(GetApplicationPath())
                .AddJsonFile(@"ClientSettings.json", true)
                .AddCommandLine(args)
                .Build();
        }

        [NotNull]
        private static string GetApplicationPath()
        {
            var assembly = Assembly.GetCallingAssembly();

            return Path.GetDirectoryName(assembly.Location) ?? string.Empty;
        }

        [NotNull]
        public static IServiceProvider ConfigureDependencyInjection([NotNull] params string[] args)
        {
            var services = new ServiceCollection();

            services.AddSingleton(services);

            var commandLineArguments = new List<string>();

            if (args.Any())
            {
                if (TryProcessArgument(Argument.ConfigFileFolder, args, out var commandLineArgument))
                {
                    commandLineArguments.Add($"DataConfiguration:{Argument.ConfigFileFolder}={commandLineArgument}");
                }

                if (TryProcessArgument(Argument.DefaultDatabase, args, out commandLineArgument))
                {
                    commandLineArguments.Add($"DataConfiguration:{Argument.DefaultDatabase}={commandLineArgument}");
                }
            }

            var configuration = GetConfigurationRoot(commandLineArguments.ToArray());

            // client classes
            services
                .AddSingleton<IDatabaseStructure, DatabaseStructure>()
                .AddSingleton<IBaseClient, SqlClient>()
                .AddSingleton<IDataRepository, SqlClientRepository>()
                .AddSingleton<IClientConfig>(new ClientConfig(configuration));

            return services.BuildServiceProvider();
        }

        private static bool TryProcessArgument(Argument argument, [NotNull] IEnumerable<string> args, [NotNull] out string commandLineArgument)
        {
            commandLineArgument = string.Empty;

            var arg = @"--" + argument;

            var executeArg = args
                .Where(x => x.StartsWith(arg, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.TrimStartIgnoreCase(arg, true))
                .Where(x => x.StartsWith(" ") || x.TrimStart().StartsWith("=") || x.TrimStart().StartsWith(":"))
                .Select(x => x.TrimStart().TrimStart("=").TrimStart(":"))
                .ToList();

            if (!executeArg.Any())
            {
                return false;
            }

            switch (argument)
            {
                case Argument.ConfigFileFolder:
                case Argument.DefaultDatabase:
                    commandLineArgument = executeArg.First();
                    return true;
            }

            return false;
        }

    }
}
