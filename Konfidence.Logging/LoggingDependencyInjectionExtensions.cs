using Konfidence.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;

namespace Konfidence.Logging;

public static class LoggingDependencyInjectionExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        public IServiceCollection AddLoggingServices(string applicationName, string logBasePath = "")
        {
            if (applicationName.IsAssigned())
            {
                Log.Logger = logBasePath.IsAssigned()
                    ? ApplicationLoggerFactory.GetLogger(applicationName, logBasePath)
                    : ApplicationLoggerFactory.GetLogger(applicationName, string.Empty, string.Empty);
            }

            serviceCollection.TryAddSingleton<IApplicationLoggerFactory, ApplicationLoggerFactory>();
            serviceCollection.TryAddTransient<IApplicationLogger, ApplicationLogger>();

            return serviceCollection;
        }
    }
}
