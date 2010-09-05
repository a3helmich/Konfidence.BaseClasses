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
        private string _UserName = string.Empty;
        private string _Password = string.Empty;

        private bool _PingSucceeded = false;

        internal static bool VerifyDatabaseServer(string databaseServerName, string userName, string password)
        {
            SqlServerSmo executer = new SqlServerSmo();

            return executer.PingSqlServerVersion(databaseServerName, userName, password);
        }

        private bool PingSqlServerVersion(string databaseServerName, string userName, string password)
        {
            if (IsAssigned(databaseServerName))
            {
                _DatabaseServerName = databaseServerName;
                _UserName = userName;
                _Password = password;

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
                string result;

                if (IsAssigned(_UserName) && IsAssigned(_Password))
                {
                    ServerConnection serverConnection = new ServerConnection(_DatabaseServerName, _UserName, _Password);

                    serverConnection.LoginSecure = false;

                    Server server = new Server(serverConnection);

                    result = server.PingSqlServerVersion(_DatabaseServerName, _UserName, _Password).ToString();
                }
                else
                {
                    Server server = new Server();

                    result = server.PingSqlServerVersion(_DatabaseServerName).ToString();
                }

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

        internal static bool FindDatabase(string databaseServerName, string databaseName, string userName, string password)
        {
            Server server;

            if (IsAssigned(userName) && IsAssigned(password))
            {
                ServerConnection serverConnection = new ServerConnection(databaseServerName, userName, password);

                serverConnection.LoginSecure = false;

                server = new Server(serverConnection);
            }
            else
            {
                server = new Server();
            }

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
