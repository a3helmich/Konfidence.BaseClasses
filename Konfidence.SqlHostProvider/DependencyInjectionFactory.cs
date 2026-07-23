using System;
using System.Collections.Generic;
using System.Linq;
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

        internal static string GetApplicationPath()
        {
            return AppContext.BaseDirectory;
        }

        public static IServiceProvider ConfigureDependencyInjection(params string[] args)
        {
            ServiceCollection services = new();

            services.AddSingleton(services);

            List<string> commandLineArguments = [];

            if (args.Any())
            {
                if (args.TryParseArgument(Argument.ConfigFileFolder, out string commandLineArgument))
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
