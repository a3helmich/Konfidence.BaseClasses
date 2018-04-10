using System;
using System.Data;
using System.Data.SqlClient;
using Konfidence.Base;
using Konfidence.SqlHostProvider.Exceptions;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Konfidence.SqlHostProvider.SqlServerManagement
{
    public class SqlServerCheck
    {
        public static bool VerifyDatabaseServer(Database databaseInstance)
        {
            var serverName = string.Empty;
            var databaseName = string.Empty;
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

                        databaseName = paramParts[1];
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

            if (!SqlServerSmo.VerifyDatabaseServer(serverName, userName, password))
            {
                throw new SqlClientException("Connection timeout (> 1500ms), Database Server " + serverName + " not found");
            }

            if (!SqlServerSmo.FindDatabase(serverName, databaseName, userName, password))
            {
                throw new SqlClientException("Database " + databaseName + " does not exist");
            }

            return true;
        }
    }
}
