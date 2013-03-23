namespace Konfidence.Base
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
        public const string URL_MENU_PAGE = "UrlMenuPage";

		private string _FullName = string.Empty;
		private string _Email = string.Empty;

	    #region properties
		public string FullName
		{
			get
			{
				return _FullName;
			}
			set
			{
				_FullName = value;
			}
		}

		public string Email
		{
			get
			{
				return _Email;
			}
			set
			{
				_Email = value;
			}
		}

	    public bool IsAdministrator { get; set; }

	    #endregion
	
        public InternalSessionAccount()
        {
            IsAdministrator = false;
        }
    }
}
