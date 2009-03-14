using Konfidence.BaseData;

namespace Konfidence.UserControlHelpers
{
	public class BaseConfigMailDataItem : BaseDataItem
	{
		private const string CONFIG_EMAIL_GET = "Config_EMail_Get";

		private const string MAIL_HOST = "MailHost";
		private const string MAIL_USER = "MailUser";
		private const string MAIL_PASSWORD = "MailPassword";
		private const string MAIL_FROM = "MailFrom";

		private string _MailHost = string.Empty;
		private string _MailUser = string.Empty;
		private string _MailPassword = string.Empty;
		private string _FromAddress = string.Empty;

		#region properties

		public string MailHost
		{
			get { return _MailHost; }
		}

		public string FromAddress
		{
			get { return _FromAddress; }
		}

		public string MailUser
		{
			get { return _MailUser; }
		}

		public string MailPassword
		{
			get { return _MailPassword; }
		}

		#endregion

		public BaseConfigMailDataItem()
		{
			GetItem(CONFIG_EMAIL_GET);
		}

		protected override void GetData()
		{
			_MailHost = GetFieldString(MAIL_HOST);
			_MailUser = GetFieldString(MAIL_USER);
			_MailPassword = GetFieldString(MAIL_PASSWORD);
			_FromAddress = GetFieldString(MAIL_FROM);
		}
	}
}