using JetBrains.Annotations;

namespace Konfidence.BaseUserControlHelpers.Login
{
	public class InternalSessionAccount
	{
        // application 
        public const string FROM_URL = "FromUrlPage";
        public const string ADMINISTRATOR_REQUIRED = "AdministratorRequired";
        public const string LOG_ON_ERROR = "LogOnError";
        //  account
        public const string ACCOUNT_OBJECT = "AccountObject";
        public const string CURRENT_ACCOUNT = "CurrentAccountDataItem";
        // menu
	    [UsedImplicitly]
        public const string URL_MENU_PAGE = "UrlMenuPage";

	    public string FullName { get; set; } = string.Empty;

	    public string Email { get; set; } = string.Empty;

	    public bool IsAdministrator { get; set; }

        public InternalSessionAccount()
        {
            IsAdministrator = false;
        }
    }
}
