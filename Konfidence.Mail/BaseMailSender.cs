using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Konfidence.Base;

namespace Konfidence.Mail
{
	public class BaseMailSender : BaseItem
	{
        private string _FromAddress = string.Empty;
        private string _MailHost = string.Empty;
        private string _MailUser = string.Empty;
        private string _MailPassword = string.Empty;

        private BaseMailSender()
        {
        }

        public BaseMailSender(string fromAddress, string mailHost, string mailUser, string mailPassword)
        {
            _FromAddress = fromAddress;
            _MailHost = mailHost;
            _MailUser = mailUser;
            _MailPassword = mailPassword;
        }

		public bool SendEmail(string toEmailAddress, string subject, string mailBody)
		{
			return SendEmail(toEmailAddress, subject, mailBody, true);
		}
		
		public bool SendEmail(string toEmailAddress, string subject, string mailBody, bool bodyIsHtml)
		{
			SmtpClient smtpClient;
			MailMessage mailMessage;

			MailAddress mailFrom = new MailAddress(_FromAddress);
			MailAddress mailTo = new MailAddress(toEmailAddress);

			mailMessage = new MailMessage(mailFrom, mailTo);
			smtpClient = new SmtpClient(_MailHost);

			mailMessage.Body = mailBody;
			mailMessage.IsBodyHtml = bodyIsHtml;

			mailMessage.Subject = subject;

			NetworkCredential basicAuthenticationInfo = new NetworkCredential(_MailUser, _MailPassword);

			smtpClient.UseDefaultCredentials = false;
			smtpClient.Credentials = basicAuthenticationInfo;

			try
			{
				smtpClient.Send(mailMessage);
			}
			catch  // TODO: if this happens then try to figure out why sending fails
			{
				return false;
			}
			return true;
		}

		private string Replace(string line, string tag, string subString)
		{
			return Regex.Replace(line, tag, subString, RegexOptions.IgnoreCase);
		}
	}
}
