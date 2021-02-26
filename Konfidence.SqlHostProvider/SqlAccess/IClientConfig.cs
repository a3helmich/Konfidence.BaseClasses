using System.Collections.Generic;

namespace Konfidence.SqlHostProvider.SqlAccess
{
    public interface IClientConfig
    {
        string DefaultDatabase { get; set; }

        string ConfigFileFolder { get; set; }

        bool UseEnvironmentSetting { get; set; }

        List<ConfigConnectionString> Connections { get; set; }
    }
}