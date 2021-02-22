using System;
using System.Linq;
using System.Threading.Tasks;
using Konfidence.Base;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Konfidence.SqlHostProvider.SqlServerManagement
{
    internal static class SqlServerInstance 
    {
        internal static async Task<bool> VerifyDatabaseServer(string databaseServerName, string userName, string password)
        {
            return await PingSqlServerVersionAsync(databaseServerName, userName, password);
        }

        private static async Task<bool> PingSqlServerVersionAsync(string databaseServerName, string userName, string password)
        {
            var task = new Task<bool>(() => PingSqlServerVersion(databaseServerName, userName, password));

            task.Start();

            return await task;
        }

        private static bool PingSqlServerVersion(string databaseServerName, string userName, string password)
        {
            try
            {
                ServerConnection serverConnection;

                if (userName.IsAssigned() && password.IsAssigned())
                {
                    serverConnection = new ServerConnection(databaseServerName, userName, password)
                    {
                        LoginSecure = false
                    };

                    return serverConnection.ServerVersion.IsAssigned();
                }

                serverConnection = new ServerConnection(databaseServerName);

                return serverConnection.ServerVersion.IsAssigned();
            }
            catch (FailedOperationException cfEx)
            {
                if (cfEx.InnerException != null)
                {
                    throw cfEx.InnerException;
                }
            }
            catch (ConnectionFailureException)
            {
                throw;
            }
            catch (Exception)
            {
                // if this fails, a timeout has already occured
            }

            return false;
        }

        internal static bool TryFindDatabase(string databaseServerName, string databaseName, string userName, string password)
        {
            Server server = null;

            if (userName.IsAssigned() && password.IsAssigned())
            {
                var serverConnection = new ServerConnection(databaseServerName, userName, password)
                {
                    LoginSecure = false
                };

                server = new Server(serverConnection);
            }

            if (!server.IsAssigned())
            {
                server = new Server(databaseServerName);
            }

            return server
                .Databases
                .Cast<Database>()
                .Select(database => database.Name)
                .Any(name => name.Equals(databaseName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
