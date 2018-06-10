using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Konfidence.Base;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Konfidence.SqlHostProvider.SqlServerManagement
{
    internal class SqlServerInstance 
    {
        private string _databaseServerName = string.Empty;
        private string _userName = string.Empty;
        private string _password = string.Empty;

        private bool _pingSucceeded;

        public SqlServerInstance()
        {
            _pingSucceeded = false;
        }

        internal static bool VerifyDatabaseServer(string databaseServerName, string userName, string password)
        {
            var executer = new SqlServerInstance();

            return executer.PingSqlServerVersion(databaseServerName, userName, password);
        }

        private bool PingSqlServerVersion(string databaseServerName, string userName, string password)
        {
            if (databaseServerName.IsAssigned())
            {
                _databaseServerName = databaseServerName;
                _userName = userName;
                _password = password;

                var executerThread = new Thread(PingSqlServerVersionExecuter);

                executerThread.Start();
                if (Debugger.IsAttached)
                {
                    executerThread.Join();
                }
                else
                {
                    executerThread.Join(3000); // 1,5 seconde genoeg, of moet dit aanpasbaar zijn?
                                               // NB. the thread is not going to stop immediately -> the application will not stop right away. 
                                               // but the response is really fast.
                }

                return _pingSucceeded;
            }

            return true;
        }

        private void PingSqlServerVersionExecuter()
        {
            _pingSucceeded = false;

            try
            {
                if (_userName.IsAssigned() && _password.IsAssigned())
                {
                    var serverConnection = new ServerConnection(_databaseServerName, _userName, _password)
                    {
                        LoginSecure = false
                    };

                    var server = new Server(serverConnection);

                    server.PingSqlServerVersion(_databaseServerName, _userName, _password);
                }
                else
                {
                    var server = new Server();

                    server.PingSqlServerVersion(_databaseServerName);
                }

                _pingSucceeded = true;
            }
            catch (FailedOperationException cfEx)
            {
                if (cfEx.InnerException != null) throw cfEx.InnerException;
            }
            catch (Exception)
            {
                // if this fails, a timeout has already occured
            }

        }

        internal static bool FindDatabase(string databaseServerName, string databaseName, string userName, string password)
        {
            Server server;

            if (userName.IsAssigned() && password.IsAssigned())
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

            var foundDatabase = databaseList.Any(name => name.Equals(databaseName, StringComparison.OrdinalIgnoreCase));

            return foundDatabase;
        }
    }
}
