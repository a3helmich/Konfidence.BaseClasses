using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Management.Smo;

namespace Konfidence.BaseData.SqlServerManagement
{
    internal class SqlServerSmo
    {
        internal static bool VerifyDatabaseServer(string databaseServerName)
        {
            Server server = new Server(databaseServerName);

            string result = server.PingSqlServerVersion(databaseServerName).ToString();

            return false;
        }
    }
}
