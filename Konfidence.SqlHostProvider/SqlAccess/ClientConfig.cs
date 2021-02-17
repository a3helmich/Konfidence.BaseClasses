using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace Konfidence.SqlHostProvider.SqlAccess
{
    public class ClientConfig : IClientConfig
    {
        public string DefaultDatabase { get; set; } = string.Empty;

        public string ConfigFileFolder { get; set; } = string.Empty;

        public ClientConfig([NotNull] IConfiguration configuration)
        {
            var section = configuration.GetSection(@"DataConfiguration");

            section.Bind(this);
        }
    }
}