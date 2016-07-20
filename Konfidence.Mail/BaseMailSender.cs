using System.Net;
using System.Net.Mail;
using Konfidence.Base;

namespace Konfidence.Mail
{
	public class BaseMailSender : BaseItem
	{
        private readonly string _FromAddress = string.Empty;
        private readonly string _MailHost = string.Empty;
        private readonly string _MailUser = string.Empty;
        private readonly string _MailPassword = string.Empty;

	    public BaseMailSender(string fromAddress, string mailHost, string mailUser, string mailPassword)
        {
            _FromAddress = fromAddress;
            _MailHost = mailHost;
            _MailUser = mailUser;
            _MailPassword = mailPassword;
        }

		public bool SendEmail(string toEmailAddress, string subject, string mailBody)
		{
			return SendEmail(toEmailAddress, subject, mailBody, true, string.Empty);
		}
		
		public bool SendEmail(string toEmailAddress, string subject, string mailBody, bool bodyIsHtml, string fileName)
		{
		    var mailFrom = new MailAddress(_FromAddress);
			var mailTo = new MailAddress(toEmailAddress);

			var mailMessage = new MailMessage(mailFrom, mailTo);
			var smtpClient = new SmtpClient(_MailHost);

			mailMessage.Body = mailBody;
			mailMessage.IsBodyHtml = bodyIsHtml;

			mailMessage.Subject = subject;

            if (fileName.IsAssigned())
            {
                var attachment = new Attachment(fileName);

                mailMessage.Attachments.Add(attachment);
            }

			var basicAuthenticationInfo = new NetworkCredential(_MailUser, _MailPassword);

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
	}
}
