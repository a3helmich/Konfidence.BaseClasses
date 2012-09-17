namespace Konfidence.Base
{
	public class InternalSessionAccount
	{
        // application 
        public const string FromUrl = "FromUrlPage";
        public const string AdministratorRequired = "AdministratorRequired";
        public const string LogOnError = "LogOnError";
        //  account
        public const string AccountObject = "AccountObject";
        public const string CurrentAccount = "CurrentAccountDataItem";
        // menu
        public const string UrlMenuPage = "UrlMenuPage";

		private string _FullName = string.Empty;
		private string _Email = string.Empty;
		private bool _Administrator = false;

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

		public bool IsAdministrator
		{
			get
			{
				return _Administrator;
			}
			set
			{
				_Administrator = value;
			}
		}
		#endregion
	}
}
