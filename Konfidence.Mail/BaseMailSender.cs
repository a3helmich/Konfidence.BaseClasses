using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Konfidence.Base;

namespace Konfidence.Mail
{
	public class BaseMailSender : BaseItem
	{
		protected static bool SendEmail(string toEmailAddress, string subject, string mailBody)
		{
			return SendEmail(toEmailAddress, subject, mailBody, true);
		}
		
		protected static bool SendEmail(string toEmailAddress, string subject, string mailBody, bool bodyIsHtml)
		{
			SmtpClient smtpClient;
			MailMessage mailMessage;

			BaseConfigMailDataItem baseConfigMailDataItem = new BaseConfigMailDataItem();

			MailAddress mailFrom = new MailAddress(baseConfigMailDataItem.FromAddress);
			MailAddress mailTo = new MailAddress(toEmailAddress);

			mailMessage = new MailMessage(mailFrom, mailTo);
			smtpClient = new SmtpClient(baseConfigMailDataItem.MailHost);

			mailMessage.Body = mailBody;
			mailMessage.IsBodyHtml = bodyIsHtml;

			mailMessage.Subject = subject;

			NetworkCredential basicAuthenticationInfo = new NetworkCredential(baseConfigMailDataItem.MailUser, baseConfigMailDataItem.MailPassword);

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

		protected static string Replace(string line, string tag, string subString)
		{
			return Regex.Replace(line, tag, subString, RegexOptions.IgnoreCase);
		}
	}
}
