using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Konfidence.BaseUserControlHelpers.DbSiteMapProvider
{
    public class DbSiteMapProviderLocal : DbSiteMapProvider
    {
        public DbSiteMapProviderLocal()
        {
            LoggedOn = false;
            Administrator = false;
            IsLocal = true;
        }
    }
}
