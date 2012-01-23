using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Konfidence.BaseUserControlHelpers.DbSiteMapProvider
{
    public class DbSiteMapProviderInternal : DbSiteMapProvider
    {
        public DbSiteMapProviderInternal()
        {
            LoggedOn = true;
            Administrator = false;
            IsLocal = false;
        }
    }
}
