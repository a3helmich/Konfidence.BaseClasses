using System;
using System.IO;
using Konfidence.Base;
using Konfidence.Logging.Inspection;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Konfidence.Logging;

public sealed class ApplicationLoggerFactory : IApplicationLoggerFactory
{
    private const string OutputTemplate = "{Timestamp:HH:mm:ss.fff} [{Level}] {Message:lj}{NewLine}{Exception}";
    private const string OutputTemplateWithProperties = "{Timestamp:HH:mm:ss.fff} [{Level}] {Properties} {Message:lj}{NewLine}{Exception}";
    private const string FileOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message:lj}{NewLine}{Exception}";
    private const string FileOutputTemplateWithProperties = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Properties} {Message:lj}{NewLine}{Exception}";

    public IApplicationLogger CreateLogger(
        Type serviceType,
        LogEventLevel logEventLevel)
    {
        return CreateLogger(serviceType, Log.Logger, logEventLevel);
    }

    public IApplicationLogger CreateLogger(
        Type serviceType,
        ILogger logger,
        LogEventLevel logEventLevel)
    {
        return new ApplicationLogger(serviceType, logger, logEventLevel);
    }

    internal static ILogger GetLogger(string applicationName, string customLocation, string customSource)
    {
        return customLocation.IsAssigned() && customSource.IsAssigned()
            ? GetCustomLoggerConfiguration(applicationName, customLocation, customSource)
            : GetLoggerConfiguration(applicationName, Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
    }

    internal static ILogger GetLogger(string applicationName, string logBasePath)
    {
        return GetLoggerConfiguration(applicationName, logBasePath);
    }

    public static ILogger GetFileOnlyLogger(string applicationName, string location)
    {
        string customLocation = Path.Combine(location, applicationName);

        customLocation.TryCreateAndValidateDirectory();

        FileSinkOptions customOptions = CreateFileSinkOptions(customLocation, $"{{Timestamp:yyyy-MM-dd HH:mm:ss.fff}} {applicationName} {{Message:lj}}{{NewLine}}{{Exception}}");

        return new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.File(
                path: customOptions.Path,
                rollingInterval: RollingInterval.Day,
                shared: true,
                outputTemplate: customOptions.OutputTemplate,
                restrictedToMinimumLevel: LogEventLevel.Debug)
            .CreateLogger();
    }

    private static ILogger GetLoggerConfiguration(string applicationName, string baseLocation)
    {
        string location = Path.Combine(Path.GetFullPath(baseLocation), applicationName);

        LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Logger(configuration => ApplyApplicationSinks(
                configuration.Filter.ByIncludingOnly(ShowSourceContext),
                    location,
                    OutputTemplateWithProperties,
                    FileOutputTemplateWithProperties))
            .WriteTo.Logger(configuration => ApplyApplicationSinks(
                configuration.Filter.ByExcluding(ShowSourceContext),
                    location,
                    OutputTemplate,
                    FileOutputTemplate));

        SelfLog.Enable(Console.Out);

        return loggerConfiguration.CreateLogger();
    }

    private static ILogger GetCustomLoggerConfiguration(string applicationName, string customLocation, string customSource)
    {
        string location = Path.Combine(Path.GetFullPath(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)), applicationName);

        customLocation = Path.Combine(customLocation, applicationName);

        customLocation.TryCreateAndValidateDirectory();

        FileSinkOptions options = CreateFileSinkOptions(location, $"{{Timestamp:yyyy-MM-dd HH:mm:ss.fff}} {applicationName} {{Message:lj}}{{NewLine}}{{Exception}}");
        FileSinkOptions customOptions = CreateFileSinkOptions(customLocation, $"{{Timestamp:yyyy-MM-dd HH:mm:ss.fff}} {applicationName} {{Message:lj}}{{NewLine}}{{Exception}}");

        LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Information)
            .Enrich.FromLogContext();

        loggerConfiguration
            .WriteTo.Logger(configuration => configuration
                .Filter.ByIncludingOnly(e => FromMySource(e, customSource))
                .WriteTo.Async(x =>
                    x.File(
                        path: customOptions.Path,
                        rollingInterval: RollingInterval.Day,
                        shared: true,
                        outputTemplate: customOptions.OutputTemplate,
                        restrictedToMinimumLevel: LogEventLevel.Debug
                        )))
            .WriteTo.Logger(configuration => configuration
                .Filter.ByExcluding(e => FromMySource(e, customSource))
                .WriteTo.Console(
                    outputTemplate: "{Timestamp:HH:mm:ss.fff} [{Level}] {Message:lj}{NewLine}{Exception}",
                    restrictedToMinimumLevel: LogEventLevel.Debug,
                    standardErrorFromLevel: LogEventLevel.Error)
                .WriteTo.File(
                    path: options.Path,
                    rollingInterval: RollingInterval.Day,
                    shared: true,
                    outputTemplate: options.OutputTemplate,
                    restrictedToMinimumLevel: LogEventLevel.Debug)
                .WriteTo.Debug(
                    outputTemplate: "{Timestamp:HH:mm:ss.fff} [{Level}] {Message:lj}{NewLine}{Exception}",
                    restrictedToMinimumLevel: LogEventLevel.Debug));

        SelfLog.Enable(Console.Out);

        ILogger applicationLogger = loggerConfiguration.CreateLogger();

        ILogger wrapLogger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Logger(configuration => configuration
                .Filter.ByIncludingOnly(e => FromMySource(e, customSource))
                .WriteTo.Sink(new InspectingSink(applicationLogger, customOptions)))
            .WriteTo.Logger(configuration => configuration
                .Filter.ByExcluding(e => FromMySource(e, customSource))
                .WriteTo.Sink(new InspectingSink(applicationLogger, options)))
            .CreateLogger();

        return wrapLogger;
    }

    private static bool FromMySource(LogEvent source, string customSource)
    {
        return source.Properties.TryGetValue("SourceContext", out LogEventPropertyValue? value) && value.ToString().TrimStart("\"").StartsWith(customSource);
    }

    private static bool ShowSourceContext(LogEvent logEvent)
    {
        return logEvent.Level is LogEventLevel.Verbose or LogEventLevel.Debug or LogEventLevel.Error or LogEventLevel.Fatal;
    }

    private static void ApplyApplicationSinks(
        LoggerConfiguration loggerConfiguration,
        string location,
        string outputTemplate,
        string fileOutputTemplate)
    {
        loggerConfiguration
            .WriteTo.Console(
                theme: AnsiConsoleTheme.Literate,
                outputTemplate: outputTemplate,
                restrictedToMinimumLevel: LogEventLevel.Debug,
                standardErrorFromLevel: LogEventLevel.Error)
            .WriteTo.File(
                Path.Combine(location, "DayTrading-.log"),
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Debug,
                outputTemplate: fileOutputTemplate,
                shared: true)
            .WriteTo.Debug(
                outputTemplate: outputTemplate,
                restrictedToMinimumLevel: LogEventLevel.Debug);
    }

    private static FileSinkOptions CreateFileSinkOptions(string location, string outputTemplate)
    {
        return new FileSinkOptions
        {
            Path = Path.Combine(location, "DayTrading-.log"),
            OutputTemplate = outputTemplate
        };
    }
}
