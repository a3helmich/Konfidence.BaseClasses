using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;

namespace Konfidence.BaseData.SqlServerManagement
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

            if (sqlConnection != null)
            {
                string[] connectionParameters = sqlConnection.ConnectionString.Split(';'); 

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

                    if (param.ToLower().StartsWith("user id="))
                    {
                        string[] paramParts = param.Split('=');

                        userName = paramParts[1];
                    }

                    if (param.ToLower().StartsWith("password="))
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
