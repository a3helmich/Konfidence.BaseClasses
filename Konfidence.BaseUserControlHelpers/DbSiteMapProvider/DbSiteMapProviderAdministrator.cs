using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Konfidence.BaseUserControlHelpers.DbSiteMapProvider
{
    public class DbSiteMapProviderAdministrator : DbSiteMapProvider
    {
        public DbSiteMapProviderAdministrator()
        {
            LoggedOn = true;
            Administrator = true;
            IsLocal = false;
        }
    }
}
