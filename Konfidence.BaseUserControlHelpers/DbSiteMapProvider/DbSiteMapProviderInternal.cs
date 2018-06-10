using JetBrains.Annotations;

namespace Konfidence.BaseUserControlHelpers.DbSiteMapProvider
{
    [UsedImplicitly]
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
