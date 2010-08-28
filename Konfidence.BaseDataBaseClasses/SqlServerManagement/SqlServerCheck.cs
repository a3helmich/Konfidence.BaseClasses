using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Konfidence.BaseData.SqlServerManagement
{
    internal class SqlServerCheck
    {
        public static bool VerifyDatabaseServer(Database databaseInstance)
        {
            string serverName = string.Empty;
            string databaseName = string.Empty;

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

            if (!SqlServerSmo.VerifyDatabaseServer(serverName))
            {
                throw new SqlHostException("Connection timeout (> 5000ms), Database Server " + serverName + " not found");
            }

            if (!SqlServerSmo.FindDatabase(serverName, databaseName))
            {
                throw new SqlHostException("Database " + databaseName + " does not exist");
            }

            return true;
        }
    }
}
