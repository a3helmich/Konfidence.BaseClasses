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

                return _PingSucceeded;
            }

            return true;
        }

        private void PingSqlServerVersionExecuter()
        {
            _PingSucceeded = false;

            Server server = new Server(_DatabaseServerName);

            string result = server.PingSqlServerVersion(_DatabaseServerName).ToString();

            _PingSucceeded = true;
        }
    }
}
