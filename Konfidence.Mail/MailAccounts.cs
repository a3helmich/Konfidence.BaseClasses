using System.Collections.Generic;
using JetBrains.Annotations;

namespace Konfidence.Mail
{
    public class MailAccounts
    {
        public List<MailAccount> Accounts { get; init; } = [];
    }

    public class MailAccount
    {
        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        [UsedImplicitly]
        public string Server { get; set; } = string.Empty;
    }
}
