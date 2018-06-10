using JetBrains.Annotations;

namespace Konfidence.BaseUserControlHelpers.DbSiteMapProvider
{
    [UsedImplicitly]
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
