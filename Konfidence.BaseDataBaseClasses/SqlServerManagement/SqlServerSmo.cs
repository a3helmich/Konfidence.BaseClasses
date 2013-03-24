using System.Collections.Generic;
using Microsoft.SqlServer.Management.Smo;
using System.Threading;
using Konfidence.Base;
using Microsoft.SqlServer.Management.Common;

namespace Konfidence.BaseData.SqlServerManagement
{
    internal class SqlServerSmo: BaseItem
    {
        private string _DatabaseServerName = string.Empty;
        private string _UserName = string.Empty;
        private string _Password = string.Empty;

        private bool _PingSucceeded;

        public SqlServerSmo()
        {
            _PingSucceeded = false;
        }

        internal static bool VerifyDatabaseServer(string databaseServerName, string userName, string password)
        {
            var executer = new SqlServerSmo();

            return executer.PingSqlServerVersion(databaseServerName, userName, password);
        }

        private bool PingSqlServerVersion(string databaseServerName, string userName, string password)
        {
            if (!IsEmpty(databaseServerName))
            {
                _DatabaseServerName = databaseServerName;
                _UserName = userName;
                _Password = password;

                var executerThread = new Thread(PingSqlServerVersionExecuter);

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
                if (!IsEmpty(_UserName) && !IsEmpty(_Password))
                {
                    var serverConnection = new ServerConnection(_DatabaseServerName, _UserName, _Password)
                        {
                            LoginSecure = false
                        };

                    var server = new Server(serverConnection);

                    server.PingSqlServerVersion(_DatabaseServerName, _UserName, _Password);
                }
                else
                {
                    var server = new Server();

                    server.PingSqlServerVersion(_DatabaseServerName);
                }

                _PingSucceeded = true;
            }
            catch (FailedOperationException cfEx)
            {
                throw cfEx.InnerException;
            }
            catch
            {
                // if this fails, a timeout has already occured
            }

        }

        internal static bool FindDatabase(string databaseServerName, string databaseName, string userName, string password)
        {
            Server server;

            if (!IsEmpty(userName) && !IsEmpty(password))
            {
                var serverConnection = new ServerConnection(databaseServerName, userName, password)
                    {
                        LoginSecure = false
                    };

                server = new Server(serverConnection);
            }
            else
            {
                server = new Server();
            }

            var databaseList = new List<string>();

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
