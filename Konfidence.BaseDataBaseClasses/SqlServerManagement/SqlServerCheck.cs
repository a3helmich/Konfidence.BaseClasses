using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Konfidence.BaseData.SqlServerManagement
{
    internal class SqlServerCheck
    {
        public static bool VerifyDatabase(Database databaseInstance)
        {
            string serverName = string.Empty;

            string[] connectionParameters = databaseInstance.ConnectionStringWithoutCredentials.Split(';'); 
            foreach(string param in connectionParameters)
            {
                if (param.ToLower().StartsWith("server="))
                {
                    string[] paramParts = param.Split('=');

                    serverName = paramParts[1];
                }
            }

            return SqlServerSmo.VerifyDatabaseServer(serverName);
        }
    }
}
