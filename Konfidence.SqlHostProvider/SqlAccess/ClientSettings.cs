using System.Collections.Generic;
using JetBrains.Annotations;
using Serilog.Events;

namespace Konfidence.SqlHostProvider.SqlAccess
{
    public class DataConfiguration
    {
        [UsedImplicitly] public LogEventLevel LogLevel { get; set; } = LogEventLevel.Information;

        [UsedImplicitly] public string DefaultDatabase { get; set; } = string.Empty;

        public List<ConfigConnectionString> Connections { get; set; } = new();
    }

    public class ClientSettings
    {
        [UsedImplicitly] public LogEventLevel LogLevel { get; set; } = LogEventLevel.Information;

        public DataConfiguration DataConfiguration { get; set; }
    }
}
