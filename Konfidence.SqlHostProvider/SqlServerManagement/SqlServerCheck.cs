using System;
using System.Data;
using System.Data.SqlClient;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.SqlHostProvider.Exceptions;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Konfidence.SqlHostProvider.SqlServerManagement
{
    [UsedImplicitly]
    public class SqlServerCheck
    {
        [UsedImplicitly]
        public static bool VerifyDatabaseServer(Database databaseInstance, int timeOut = 3000)
        {
            string serverName = string.Empty;
            string connectionName = string.Empty;
            string userName = string.Empty;
            string password = string.Empty;

            IDbConnection? sqlConnection = databaseInstance.CreateConnection() as SqlConnection;

            if (sqlConnection.IsAssigned())
            {
                string[] connectionParameters = sqlConnection.ConnectionString.Split(';'); 

                foreach(string? param in connectionParameters)
                {
                    if (param.StartsWith("server=", StringComparison.OrdinalIgnoreCase))
                    {
                        string[] paramParts = param.Split('=');

                        serverName = paramParts[1];
                    }

                    if (param.StartsWith("database=", StringComparison.OrdinalIgnoreCase))
                    {
                        string[] paramParts = param.Split('=');

                        connectionName = paramParts[1];
                    }

                    if (param.StartsWith("user id=", StringComparison.OrdinalIgnoreCase))
                    {
                        string[] paramParts = param.Split('=');

                        userName = paramParts[1];
                    }

                    if (param.StartsWith("password=", StringComparison.OrdinalIgnoreCase))
                    {
                        string[] paramParts = param.Split('=');

                        password = paramParts[1];
                    }
                }
            }

            if (!SqlServerInstance.VerifyDatabaseServer(serverName, userName, password).Result)
            {
                throw new SqlClientException("Connection timeout (> 1500ms), Database Server " + serverName + " not found");
            }

            if (!SqlServerInstance.TryFindDatabase(serverName, connectionName, userName, password))
            {
                throw new SqlClientException("Database " + connectionName + " does not exist");
            }

            return true;
        }
    }
}
