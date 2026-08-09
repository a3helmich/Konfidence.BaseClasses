using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.Mail;

[UsedImplicitly]
public class BaseMailSender
{
    private readonly string _fromAddress;
    private readonly string _mailHost;
    private readonly int _mailPort;
    private readonly string _mailUser;
    private readonly string _mailPassword;

    public BaseMailSender(string fromAddress, string mailHost, string mailUser, string mailPassword, int mailPort = 25)
    {
        _fromAddress = fromAddress;
        _mailHost = mailHost;
        _mailPort = mailPort;
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
        try
        {
            MailAddress mailFrom = new(_fromAddress);
            MailAddress mailTo = new(toEmailAddress);

            using MailMessage mailMessage = new(mailFrom, mailTo)
            {
                Body = mailBody,
                IsBodyHtml = bodyIsHtml,
                Subject = subject
            };

            using Attachment? attachment = fileName.IsAssigned() ? new Attachment(fileName) : null;

            if (attachment.IsAssigned())
            {
                mailMessage.Attachments.Add(attachment);
            }

            using SmtpClient smtpClient = new(_mailHost, _mailPort)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_mailUser, _mailPassword)
            };

            smtpClient.Send(mailMessage);

            return true;
        }
        catch (Exception exception)
        {
            Trace.WriteLine($"BaseMailSender.SendEmail failed: {exception}");

            return false;
        }
    }
}
