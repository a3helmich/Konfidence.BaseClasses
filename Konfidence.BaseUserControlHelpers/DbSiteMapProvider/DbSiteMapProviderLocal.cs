using JetBrains.Annotations;

namespace Konfidence.BaseUserControlHelpers.DbSiteMapProvider
{
    [UsedImplicitly]
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
