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
		public bool SendEmail(string toEmailAddress, string subject, string mailBody)
		{
			return SendEmail(toEmailAddress, subject, mailBody, true, string.Empty);
		}
		
		public bool SendEmail(string toEmailAddress, string subject, string mailBody, bool bodyIsHtml, string fileName)
		{
		    MailAddress? mailFrom = new(_fromAddress);
			MailAddress? mailTo = new(toEmailAddress);

			MailMessage? mailMessage = new(mailFrom, mailTo);
			SmtpClient? smtpClient = new(_mailHost);

			mailMessage.Body = mailBody;
			mailMessage.IsBodyHtml = bodyIsHtml;

			mailMessage.Subject = subject;

            if (fileName.IsAssigned())
            {
                Attachment? attachment = new(fileName);

                mailMessage.Attachments.Add(attachment);
            }

			NetworkCredential? basicAuthenticationInfo = new(_mailUser, _mailPassword);

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
