using System.Linq;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.SqlHostProvider.SqlConnectionManagement;

namespace Konfidence.SqlHostProvider.SqlAccess
{
    internal static class ClientConfigExtensions
    {
        public static void SetSqlApplicationSettings([NotNull] this ClientConfig clientConfig)
        {
            if (!clientConfig.DefaultDatabase.IsAssigned())
            {
                return;
            }

            var connection = clientConfig.Connections.FirstOrDefault(x => x.ConnectionName == clientConfig.DefaultDatabase);

            if (connection.IsAssigned())
            {
                ConnectionManagement.SetApplicationDatabase(clientConfig.DefaultDatabase, connection.Server, connection.ConnectionName);
            }

            ConnectionManagement.SetActiveConnection(clientConfig.DefaultDatabase);
        }

        [CanBeNull]
        public static ConfigConnectionString GetConfigConnection([NotNull] this IClientConfig clientConfig)
        {
            var connection = clientConfig
                .Connections
                .FirstOrDefault(x =>
                    clientConfig.DefaultDatabase.IsAssigned() && x.ConnectionName == clientConfig.DefaultDatabase);

            return connection;
        }
    }
}
