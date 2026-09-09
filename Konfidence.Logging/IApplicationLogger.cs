using System;
using System.Runtime.CompilerServices;
using Serilog.Events;

namespace Konfidence.Logging;

public interface IApplicationLogger
{
    void Error(Exception exception, string message = "", [CallerMemberName] string method = "");

    void Verbose(string message, [CallerMemberName] string method = "");

    void Verbose(object obj, LogAction action = LogAction.None, [CallerMemberName] string method = "");

    void Verbose(LogAction action, [CallerMemberName] string method = "");

    void Information(LogAction action, [CallerMemberName] string method = "");

    void Information(string message, [CallerMemberName] string method = "");

    void Information(object obj, LogAction action = LogAction.None, [CallerMemberName] string method = "");

    void InformationRaw(string message);
}

public interface IApplicationLoggerFactory
{
    IApplicationLogger CreateLogger(
        Type serviceType,
        LogEventLevel logEventLevel);

    IApplicationLogger CreateLogger(
        Type serviceType,
        Serilog.ILogger logger,
        LogEventLevel logEventLevel);
}
