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
    }
}
