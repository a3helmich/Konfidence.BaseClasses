namespace Konfidence.Base
{
	public class SessionAccount
	{
		private string _LoggedOn = string.Empty;
		private string _Email = string.Empty;
		private bool _Administrator = false;

		#region properties
		public string LoggedOn
		{
			get
			{
				return _LoggedOn;
			}
			set
			{
				_LoggedOn = value;
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
