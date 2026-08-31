using System;
using System.Collections.Generic;
using Konfidence.SqlHostProvider.SqlConnectionManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Konfidence.SqlHostProvider;

public static class DependencyInjectionFactory
{
    private static readonly Dictionary<string, string> SwitchMappings = new()
    {
        [$"--{Argument.ConfigFileFolder}"] = $"DataConfiguration:{Argument.ConfigFileFolder}",
        [$"--{Argument.DefaultDatabase}"] = $"DataConfiguration:{Argument.DefaultDatabase}"
    };

    internal static string GetApplicationPath()
    {
        return AppContext.BaseDirectory;
    }

    public static IServiceProvider ConfigureDependencyInjection(params string[] args)
    {
        ServiceCollection services = new();

        services.AddSingleton(services);

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(GetApplicationPath())
            .AddJsonFile(SqlConnectionConstants.DefaultConfigFileName, true)
            .AddCommandLine(args, SwitchMappings)
            .Build();

        services.AddSqlHostProviderServices(configuration);

        return services.BuildServiceProvider();
    }
}
