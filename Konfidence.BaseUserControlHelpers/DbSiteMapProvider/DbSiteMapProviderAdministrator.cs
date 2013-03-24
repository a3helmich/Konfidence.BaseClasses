namespace Konfidence.BaseUserControlHelpers.DbSiteMapProvider
{
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
