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
        public static bool VerifyDatabaseServer([NotNull] Database databaseInstance)
        {
            var serverName = string.Empty;
            var connectionName = string.Empty;
            var userName = string.Empty;
            var password = string.Empty;

            IDbConnection sqlConnection = databaseInstance.CreateConnection() as SqlConnection;

            if (sqlConnection.IsAssigned())
            {
                var connectionParameters = sqlConnection.ConnectionString.Split(';'); 

                foreach(var param in connectionParameters)
                {
                    if (param.StartsWith("server=", StringComparison.OrdinalIgnoreCase))
                    {
                        var paramParts = param.Split('=');

                        serverName = paramParts[1];
                    }

                    if (param.StartsWith("database=", StringComparison.OrdinalIgnoreCase))
                    {
                        var paramParts = param.Split('=');

                        connectionName = paramParts[1];
                    }

                    if (param.StartsWith("user id=", StringComparison.OrdinalIgnoreCase))
                    {
                        var paramParts = param.Split('=');

                        userName = paramParts[1];
                    }

                    if (param.StartsWith("password=", StringComparison.OrdinalIgnoreCase))
                    {
                        var paramParts = param.Split('=');

                        password = paramParts[1];
                    }
                }
            }

            if (!SqlServerInstance.VerifyDatabaseServer(serverName, userName, password))
            {
                throw new SqlClientException("Connection timeout (> 1500ms), Database Server " + serverName + " not found");
            }

            if (!SqlServerInstance.FindDatabase(serverName, connectionName, userName, password))
            {
                throw new SqlClientException("Database " + connectionName + " does not exist");
            }

            return true;
        }
    }
}
