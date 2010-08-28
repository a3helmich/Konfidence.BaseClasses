using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Management.Smo;
using System.Threading;
using Konfidence.Base;

namespace Konfidence.BaseData.SqlServerManagement
{
    internal class SqlServerSmo: BaseItem
    {
        private string _DatabaseServerName = string.Empty;
        private bool _PingSucceeded = false;

        internal static bool VerifyDatabaseServer(string databaseServerName)
        {
            SqlServerSmo executer = new SqlServerSmo();

            return executer.PingSqlServerVersion(databaseServerName);
        }

        private bool PingSqlServerVersion(string databaseServerName)
        {
            if (IsAssigned(databaseServerName))
            {
                _DatabaseServerName = databaseServerName;

                Thread executerThread = new Thread(PingSqlServerVersionExecuter);

                executerThread.Start();

                executerThread.Join(1000); // 1 seconde genoeg, of moet dit aanpasbaar zijn?
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
                Server server = new Server(_DatabaseServerName);

                string result = server.PingSqlServerVersion(_DatabaseServerName).ToString();

                _PingSucceeded = true;
            }
            catch
            {
                // if this fails, a timeout has already occured
            }

        }
    }
}
