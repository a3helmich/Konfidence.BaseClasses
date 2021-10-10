using System.Collections.Generic;

namespace Konfidence.Mail
{
    public class MailAccounts
    {
        public List<MailAccount> Accounts = new List<MailAccount>();
    }
    public class MailAccount
    {
        public string UserName = string.Empty;
        public string Password = string.Empty;
        public string Server = string.Empty;
    }
}
