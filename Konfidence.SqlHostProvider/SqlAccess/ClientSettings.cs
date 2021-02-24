using System.Collections.Generic;
using JetBrains.Annotations;

namespace Konfidence.SqlHostProvider.SqlAccess
{
    public class DataConfiguration
    {
        [UsedImplicitly] public string DefaultDatabase { get; set; } = string.Empty;

        public List<ConfigConnectionString> Connections { get; set; } = new();
    }

    public class ClientSettings
    {
        public DataConfiguration DataConfiguration { get; set; }
    }
}
