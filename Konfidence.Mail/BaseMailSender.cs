using System.Net;
using System.Net.Mail;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.Mail
{
    [UsedImplicitly]
	public class BaseMailSender 
	{
        private readonly string _fromAddress;
        private readonly string _mailHost;
        private readonly string _mailUser;
        private readonly string _mailPassword;

	    public BaseMailSender(string fromAddress, string mailHost, string mailUser, string mailPassword)
        {
            _fromAddress = fromAddress;
            _mailHost = mailHost;
            _mailUser = mailUser;
            _mailPassword = mailPassword;
        }

	    [UsedImplicitly]
		public bool SendEmail([NotNull] string toEmailAddress, string subject, [NotNull] string mailBody)
		{
			return SendEmail(toEmailAddress, subject, mailBody, true, string.Empty);
		}
		
		public bool SendEmail([NotNull] string toEmailAddress, string subject, [NotNull] string mailBody, bool bodyIsHtml, string fileName)
		{
		    var mailFrom = new MailAddress(_fromAddress);
			var mailTo = new MailAddress(toEmailAddress);

			var mailMessage = new MailMessage(mailFrom, mailTo);
			var smtpClient = new SmtpClient(_mailHost);

			mailMessage.Body = mailBody;
			mailMessage.IsBodyHtml = bodyIsHtml;

			mailMessage.Subject = subject;

            if (fileName.IsAssigned())
            {
                var attachment = new Attachment(fileName);

                mailMessage.Attachments.Add(attachment);
            }

			var basicAuthenticationInfo = new NetworkCredential(_mailUser, _mailPassword);

			smtpClient.UseDefaultCredentials = false;
			smtpClient.Credentials = basicAuthenticationInfo;

			try
			{
				smtpClient.Send(mailMessage);
			}
			catch  
			{
				return false;
			}
			return true;
		}
	}
}
