using System;
using System.IO;
using System.Reflection;
using JetBrains.Annotations;
using Konfidence.SqlHostProvider.SqlDbSchema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Konfidence.SqlHostProvider.SqlAccess
{
    public class DependencyInjectionFactory
    {
        private static IConfigurationRoot GetConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .SetBasePath(GetApplicationPath())
                .AddJsonFile(@"ClientSettings.json", true)
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
            var services = new ServiceCollection();

            var configuration = GetConfigurationRoot();

            // special classes 
            services
                .AddSingleton<IConfiguration>(configuration);

            // client classes
            services
                .AddSingleton<IDatabaseStructure, DatabaseStructure>()
            //    .AddSingleton<IApplicationConfigurationFactory, ApplicationConfigurationFactory>()
                .AddSingleton<IClientConfig, ClientConfig>();

            // dto factories
            //services
            //    .AddSingleton(x =>
            //    {
            //        var applicationConfigurationFactory = x.GetService<IApplicationConfigurationFactory>();

            //        return applicationConfigurationFactory?.GetApplicationConfiguration();
            //    });

            var serviceProvider = services.BuildServiceProvider();

            services.AddSingleton(serviceProvider);

            return serviceProvider;
        }
    }
}
