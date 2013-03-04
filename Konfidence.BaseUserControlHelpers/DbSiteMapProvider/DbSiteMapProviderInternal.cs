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
