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
