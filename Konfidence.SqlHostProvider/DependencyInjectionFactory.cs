using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
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
        public static IServiceProvider ConfigureDependencyInjection()
        {
            return ConfigureDependencyInjection(new string[] { });
        }

        [NotNull]
        public static IServiceProvider ConfigureDependencyInjection([NotNull] string[] args)
        {
            var services = new ServiceCollection();

            services.AddSingleton(services);

            var commandLineArguments = new List<string>();

            if (args.Any())
            {
                commandLineArguments.Add($"DataConfiguration:ConfigFileFolder={args[0]}");
            }

            var configuration = GetConfigurationRoot(commandLineArguments.ToArray());

            // special classes 
            services
                .AddSingleton<IConfiguration>(configuration);

            // client classes
            services
                .AddSingleton<IDatabaseStructure, DatabaseStructure>()
                .AddSingleton<IBaseClient, SqlClient>()
                .AddSingleton<IDataRepository, SqlClientRepository>()
            //    .AddSingleton<IApplicationConfigurationFactory, ApplicationConfigurationFactory>()
                .AddSingleton<IClientConfig, ClientConfig>();

            //IDataRepository

            // dto factories
            //services
            //    .AddSingleton(x =>
            //    {
            //        var applicationConfigurationFactory = x.GetService<IApplicationConfigurationFactory>();

            //        return applicationConfigurationFactory?.GetApplicationConfiguration();
            //    });

            return services.BuildServiceProvider();
        }
    }
}
