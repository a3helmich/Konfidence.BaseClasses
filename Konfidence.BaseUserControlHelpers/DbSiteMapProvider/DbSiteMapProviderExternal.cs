using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Konfidence.BaseUserControlHelpers.DbSiteMapProvider
{
    public class DbSiteMapProviderExternal : DbSiteMapProvider
    {
        public DbSiteMapProviderExternal()
        {
            LoggedOn = false;
            Administrator = false;
            IsLocal = false;
        }
    }
}
