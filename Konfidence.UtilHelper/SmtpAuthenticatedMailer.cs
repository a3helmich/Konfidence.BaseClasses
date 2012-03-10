//using System.Threading;
//using System.Web.Mail;

//namespace Konfidence.UtilHelper
//{
//    /// <summary>
//    /// Summary description for SmtpAuthenticatedMailer.
//    /// </summary>
//    public class SmtpAuthenticatedMailer
//    {
//        // voor de mailer is er een vs2005/2008 versie
//    private MailMessage _MailMessage = new MailMessage();

//    // TODO : moet configurable worden
//    private string _SmtpUser     = string.Empty;  // moet configurable worden
//    private string _SmtpPassword = string.Empty;  // moet configurable worden
//    private string _MailServer   = string.Empty;  // moet configurable worden 
//    private string _From         = string.Empty;  // moet configurable worden

//    private string _FieldSendUsing        = "http://schemas.microsoft.com/cdo/configuration/sendusing";
//    private string _FieldSmtpAuthenticate = "http://schemas.microsoft.com/cdo/configuration/smtpauthenticate";
//    private string _FieldSendUsername     = "http://schemas.microsoft.com/cdo/configuration/sendusername";
//    private string _FieldSendPassword     = "http://schemas.microsoft.com/cdo/configuration/sendpassword";

//    public string From
//    {
//      get { return _MailMessage.From; }
//      set 
//      { 
//        _MailMessage.From = value; 
//        _From             = value;
//      }
//    }

//    public string To
//    {
//      get { return _MailMessage.To; }
//      set { _MailMessage.To = value; }
//    }

//    public string Subject
//    {
//      get { return _MailMessage.Subject; }
//      set { _MailMessage.Subject = value; }
//    }

//    public string Body
//    {
//      get { return _MailMessage.Body; }
//      set { _MailMessage.Body = value; }
//    }

//    public void AddAttachment(string fileName)
//    {
//      MailAttachment mailAttachment = new MailAttachment(fileName);

//      _MailMessage.Attachments.Add(mailAttachment);
//    }

//    public void Send()
//    {

//      Thread sendSmtpThread = new Thread(new ThreadStart(this.SendSmtp));
      
//      if (_SmtpUser.Length > 0 && _SmtpPassword.Length > 0)
//      {
//        if (_From.Length == 0)
//          _MailMessage.From = _SmtpUser;

//        // cdoSendUsingPickup = 1 Send the message using the local SMTP service pickup directory.
//        // cdoSendUsingPort   = 2 Send the message using the network ( SMTP over the network).
//        _MailMessage.Fields[_FieldSendUsing] = 2;
//        _MailMessage.Fields[_FieldSmtpAuthenticate] = 1;
//        _MailMessage.Fields[_FieldSendUsername] = _SmtpUser;
//        _MailMessage.Fields[_FieldSendPassword] = _SmtpPassword;

//        SmtpMail.SmtpServer = _MailServer;

//        sendSmtpThread.Start();
//      }
//    }
//    private void SendSmtp()
//    {
//      SmtpMail.Send(_MailMessage);
//    }
//  }
//}
