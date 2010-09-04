using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Management.Smo;
using System.Threading;
using Konfidence.Base;
using Microsoft.SqlServer.Management.Common;
using System.Data.SqlClient;

namespace Konfidence.BaseData.SqlServerManagement
{
    internal class SqlServerSmo: BaseItem
    {
        private string _DatabaseServerName = string.Empty;
        private SqlConnection _SqlConnection = null;
        private bool _PingSucceeded = false;

        internal static bool VerifyDatabaseServer(SqlConnection sqlConnection, string databaseServerName)
        {
            SqlServerSmo executer = new SqlServerSmo();

            return executer.PingSqlServerVersion(sqlConnection, databaseServerName);
        }

        private bool PingSqlServerVersion(SqlConnection sqlConnection, string databaseServerName)
        {
            if (IsAssigned(databaseServerName))
            {
                _DatabaseServerName = databaseServerName;
                _SqlConnection = sqlConnection;

                Thread executerThread = new Thread(PingSqlServerVersionExecuter);

                executerThread.Start();

                executerThread.Join(3000); // 1,5 seconde genoeg, of moet dit aanpasbaar zijn?
                // NB. the thread is not going to stop immediately -> the application will not stop right away. 
                // but the response is really fast.

                return _PingSucceeded;
            }

            return true;
        }

        private void PingSqlServerVersionExecuter()
        {
            _PingSucceeded = false;

            try
            {
                ServerConnection serverConnection = new ServerConnection(_SqlConnection);

                Server server = new Server(serverConnection);

                string result = server.PingSqlServerVersion(_DatabaseServerName).ToString();

                _PingSucceeded = true;
            }
            catch (FailedOperationException cfEx)
            {
                throw cfEx.InnerException;
            }
            catch (Exception ex)
            {
                string test = ex.Message;
                // if this fails, a timeout has already occured
            }

        }

        internal static bool FindDatabase(SqlConnection sqlConnection, string databaseName)
        {
            ServerConnection serverConnection = new ServerConnection(sqlConnection);

            Server server = new Server(serverConnection);

            List<string> databaseList = new List<string>();

            foreach (Database database in server.Databases)
            {
                databaseList.Add(database.Name.ToLower());
            }

            if (!databaseList.Contains(databaseName.ToLower()))
            {
                return false;
            }

            return true;
        }
    }
}
