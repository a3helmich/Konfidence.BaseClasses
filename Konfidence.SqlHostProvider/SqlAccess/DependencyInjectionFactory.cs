using System;
using System.IO;
using System.Reflection;
using JetBrains.Annotations;
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
                .AddJsonFile(@"HostSettings.json", true)
                .Build();
        }

        [NotNull]
        private static string GetApplicationPath()
        {
            var assembly = Assembly.GetEntryAssembly();

            return assembly == null
                ? string.Empty
                : Path.GetDirectoryName(assembly.Location)??string.Empty;
        }

        [NotNull]
        public static IServiceProvider ConfigureDependencyInjection()
        {
            var services = new ServiceCollection();

            //var mapping = ApplicationMapping.GetMapping();
            var configuration = GetConfigurationRoot();

            // special classes 
            services
                //.AddSingleton(mapping)
                .AddSingleton<IConfiguration>(configuration);

            // client classes
            //services
            //    .AddSingleton<ClientExecuter>()
            //    .AddSingleton<IRestClient, RestClient>()
            //    .AddSingleton<IApplicationConfigurationFactory, ApplicationConfigurationFactory>()
            //    .AddSingleton<IClientConfig, ClientConfig>()
            //    .AddSingleton<IBaseRestClient, BaseRestClient>()
            //    .AddSingleton<ILicenseClient, LicenseClient>()
            //    .AddSingleton<IEncryptionPresenter, EncryptionPresenter>()
            //    .AddSingleton<IClientPresenter, ClientPresenter>();

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
