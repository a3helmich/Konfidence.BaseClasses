using System;
using System.Runtime.CompilerServices;
using Konfidence.Base;
using Serilog;
using Serilog.Events;

namespace Konfidence.Logging;

internal sealed class ApplicationLogger : IApplicationLogger
{
    private readonly ILogger _logger;
    private readonly LogEventLevel _logLevel;
    private readonly Type _serviceType;

    public ApplicationLogger()
        : this(typeof(ApplicationLogger), Log.Logger, LogEventLevel.Information)
    {
    }

    internal ApplicationLogger(
        Type serviceType,
        ILogger logger,
        LogEventLevel logLevel)
    {
        _serviceType = serviceType;
        _logger = logger;
        _logLevel = logLevel;
    }

    public void Error(Exception exception, string message = "", [CallerMemberName] string method = "")
    {
        if (_logLevel > LogEventLevel.Error)
        {
            return;
        }

        if (message.IsAssigned())
        {
            ForServiceContext().Error(exception, " {0}() {1}", method, message);

            return;
        }

        ForServiceContext().Error(exception, " {0}()", method);
    }

    public void Verbose(object obj, LogAction action = LogAction.None, [CallerMemberName] string method = "")
    {
        if (_logLevel > LogEventLevel.Verbose)
        {
            return;
        }

        string message = action == LogAction.None ? string.Empty : $"({action})";

        try
        {
            ForServiceContext().Verbose("{0}() {1} {2}", method, message, $"{Environment.NewLine}{obj.Serialize()}");
        }
        catch
        {
            ForServiceContext().Error("{0}() {1} {2}", method, message, $"{Environment.NewLine}UNABLE TO SERIALIZE OBJECT");
        }
    }

    public void Verbose(LogAction action, [CallerMemberName] string method = "")
    {
        Verbose($"({action})", method);
    }

    public void Verbose(string message, [CallerMemberName] string method = "")
    {
        if (_logLevel > LogEventLevel.Verbose)
        {
            return;
        }

        ForServiceContext().Verbose(" {0}() {1}", method, message);
    }

    public void Information(object obj, LogAction action = LogAction.None, [CallerMemberName] string method = "")
    {
        if (_logLevel > LogEventLevel.Information)
        {
            return;
        }

        string message = action == LogAction.None ? string.Empty : $"({action})";

        try
        {
            ForServiceContext().Information("{0}() {1} {2}", method, message, $"{Environment.NewLine}{obj.Serialize()}");
        }
        catch
        {
            ForServiceContext().Error("{0}() {1} {2}", method, message, $"{Environment.NewLine}UNABLE TO SERIALIZE OBJECT");
        }
    }

    public void Information(LogAction action, [CallerMemberName] string method = "")
    {
        Information($"({action})", method);
    }

    public void Information(string message, [CallerMemberName] string method = "")
    {
        if (_logLevel > LogEventLevel.Information)
        {
            return;
        }

        ForServiceContext().Information("{0}() {1}", method, message);
    }

    public void InformationRaw(string message)
    {
        if (_logLevel > LogEventLevel.Information)
        {
            return;
        }

        ForServiceContext().Information("{0}", message);
    }

    private ILogger ForServiceContext()
    {
        return _logger.ForContext(_serviceType);
    }
}
