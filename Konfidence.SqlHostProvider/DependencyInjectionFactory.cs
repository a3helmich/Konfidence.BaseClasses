using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Konfidence.Base;
using Konfidence.DatabaseInterface;
using Konfidence.SqlHostProvider.SqlAccess;
using Konfidence.SqlHostProvider.SqlConnectionManagement;
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
                .AddJsonFile(SqlConnectionConstants.DefaultConfigFileName, true)
                .AddCommandLine(args)
                .Build();
        }

        private static string GetApplicationPath()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            string directoryName = Path.GetDirectoryName(assembly.Location) ?? string.Empty;

            return directoryName;
        }

        public static IServiceProvider ConfigureDependencyInjection(params string[] args)
        {
            ServiceCollection services = new();

            services.AddSingleton(services);

            List<string> commandLineArguments = [];

            if (args.Any())
            {
                if (args.TryParseArgument(Argument.ConfigFileFolder, out string? commandLineArgument))
                {
                    commandLineArguments.Add($"DataConfiguration:{Argument.ConfigFileFolder}={commandLineArgument}");
                }

                if (args.TryParseArgument(Argument.DefaultDatabase, out commandLineArgument))
                {
                    commandLineArguments.Add($"DataConfiguration:{Argument.DefaultDatabase}={commandLineArgument}");
                }
            }

            IConfigurationRoot configuration = GetConfigurationRoot(commandLineArguments.ToArray());

            ClientConfig clientConfig = new(configuration);

            clientConfig.SetSqlApplicationSettings();

            // client classes
            services
                .AddSingleton<IDatabaseStructure, DatabaseStructure>()
                .AddSingleton<IBaseClient, SqlClient>()
                .AddSingleton<IDataRepository, SqlClientRepository>()
                .AddSingleton<IClientConfig>(clientConfig);

            return services.BuildServiceProvider();
        }
    }
}
