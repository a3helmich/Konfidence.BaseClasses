using System;
using System.Data.SqlClient;
using Konfidence.Base;
using Konfidence.BaseData;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Konfidence.Smo.SqlServerManagement
{
    internal class SqlServerCheck
    {
        public static bool VerifyDatabaseServer(Database databaseInstance)
        {
            string serverName = string.Empty;
            string databaseName = string.Empty;
            string userName = string.Empty;
            string password = string.Empty;

            var sqlConnection = databaseInstance.CreateConnection() as SqlConnection;

            if (sqlConnection.IsAssigned())
            {
                string[] connectionParameters = sqlConnection.ConnectionString.Split(';'); 

                foreach(string param in connectionParameters)
                {
                    if (param.StartsWith("server=", StringComparison.OrdinalIgnoreCase))
                    {
                        string[] paramParts = param.Split('=');

                        serverName = paramParts[1];
                    }

                    if (param.StartsWith("database=", StringComparison.OrdinalIgnoreCase))
                    {
                        string[] paramParts = param.Split('=');

                        databaseName = paramParts[1];
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

            if (!SqlServerSmo.VerifyDatabaseServer(serverName, userName, password))
            {
                throw new SqlHostException("Connection timeout (> 1500ms), Database Server " + serverName + " not found");
            }

            if (!SqlServerSmo.FindDatabase(serverName, databaseName, userName, password))
            {
                throw new SqlHostException("Database " + databaseName + " does not exist");
            }

            return true;
        }
    }
}
