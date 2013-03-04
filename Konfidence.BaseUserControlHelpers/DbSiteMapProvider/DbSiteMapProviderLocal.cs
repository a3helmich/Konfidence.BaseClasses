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
