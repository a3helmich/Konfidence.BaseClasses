using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSettingsUpdater
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
