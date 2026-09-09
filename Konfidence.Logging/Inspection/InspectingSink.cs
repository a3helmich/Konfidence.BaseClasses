using JetBrains.Annotations;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Konfidence.Logging.Inspection;

internal sealed class InspectingSink : ILogEventSink
{
    private readonly ILogger _logger;

    public FileSinkOptions Options { [UsedImplicitly] get; }

    public string RenderedMessage { [UsedImplicitly] get; private set; } = string.Empty;

    public InspectingSink(
        ILogger logger,
        FileSinkOptions options)
    {
        _logger = logger;

        Options = options;
    }

    public void Emit(LogEvent logEvent)
    {
        _logger.Write(logEvent);

        RenderedMessage = logEvent.RenderMessage();
    }
}
