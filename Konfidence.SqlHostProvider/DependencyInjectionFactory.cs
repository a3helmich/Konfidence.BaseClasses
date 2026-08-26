using System;
using System.Collections.Generic;
using System.Linq;
using Konfidence.Base;
using Konfidence.SqlHostProvider.SqlAccess;
using Konfidence.SqlHostProvider.SqlConnectionManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Konfidence.SqlHostProvider;

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

    public static IConfigurationRoot BuildConfiguration(params string[] args)
    {
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

        return GetConfigurationRoot(commandLineArguments.ToArray());
    }

    public static IServiceProvider ConfigureDependencyInjection(params string[] args)
    {
        ServiceCollection services = new();

        services.AddSingleton(services);

        services.AddSqlHostProviderServices(BuildConfiguration(args));

        return services.BuildServiceProvider();
    }
}
