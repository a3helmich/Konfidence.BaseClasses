using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace Konfidence.BaseData.SqlServerManagement
{
    internal class SqlServerCheck
    {
        public static bool VerifyDatabaseServer(Database databaseInstance)
        {
            string serverName = string.Empty;
            string databaseName = string.Empty;

            SqlConnection sqlConnection = databaseInstance.CreateConnection() as SqlConnection;
            
            string[] connectionParameters = databaseInstance.ConnectionStringWithoutCredentials.Split(';'); 

            foreach(string param in connectionParameters)
            {
                if (param.ToLower().StartsWith("server="))
                {
                    string[] paramParts = param.Split('=');

                    serverName = paramParts[1];
                }

                if (param.ToLower().StartsWith("database="))
                {
                    string[] paramParts = param.Split('=');

                    databaseName = paramParts[1];
                }
            }

            if (!SqlServerSmo.VerifyDatabaseServer(sqlConnection, serverName))
            {
                throw new SqlHostException("Connection timeout (> 1500ms), Database Server " + serverName + " not found");
            }

            if (!SqlServerSmo.FindDatabase(sqlConnection, databaseName))
            {
                throw new SqlHostException("Database " + databaseName + " does not exist");
            }

            return true;
        }
    }
}
