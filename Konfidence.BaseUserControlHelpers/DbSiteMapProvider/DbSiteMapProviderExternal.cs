using JetBrains.Annotations;

namespace Konfidence.BaseUserControlHelpers.DbSiteMapProvider
{
    [UsedImplicitly]
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
