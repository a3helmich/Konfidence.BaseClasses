using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;
using System.Web.UI;
using System.Configuration;
using System.Web;

namespace Konfidence.BaseUserControlHelpers
{
    public class BaseWebPresenter : BaseItem
    {
        private string _MenuPage = string.Empty;
        private string _AccountEmail = string.Empty;
        private string _AccountPassword = string.Empty;

        public string MenuPage
        {
            get { return ConfigurationManager.AppSettings["MenuPage"] as string; }
        }

        public string AccountEmail
        {
            get { return _AccountEmail; }
            set { _AccountEmail = value; }
        }

        public string AccountPassword
        {
            get { return _AccountPassword; }
            set { _AccountPassword = value; }
        }

        public SessionAccount SessionAccount
        {
            get { return HttpContext.Current.Session[KitSessionAccount.AccountObject] as SessionAccount; }
        }

        public bool IsLocal
        {
            get { return HttpContext.Current.Request.IsLocal; }
        }

        protected void SessionLogon(string fullName, string email, bool isAdministrator)
        {
            HttpContext.Current.Session[KitSessionAccount.AccountObject] = new SessionAccount();

            SessionAccount.FullName = fullName;
            SessionAccount.Email = email;
            SessionAccount.IsAdministrator = isAdministrator;

            if (!IsValidAccount)
            {
                LogOff();
            }
        }

        public virtual void LogOff()
        {
            if (IsAssigned(HttpContext.Current.Session[KitSessionAccount.AccountObject]))
            {
                HttpContext.Current.Session.Remove(KitSessionAccount.AccountObject);
            }
        }

        public string FromUrl
        {
            get { return HttpContext.Current.Session[KitSessionAccount.FromUrl] as string; }
            set { HttpContext.Current.Session[KitSessionAccount.FromUrl] = value; }
        }

        public string Email
        {
            get
            {
                if (IsAssigned(SessionAccount))
                {
                    return SessionAccount.Email;
                }

                return string.Empty;
            }
        }

        public bool IsLoggedIn
        {
            get
            {
                if (IsAssigned(SessionAccount))
                {
                    return true;
                }

                return false;
            }
        }

        public string LoginMessage
        {
            get
            {
                string ErrorText = HttpContext.Current.Session[KitSessionAccount.LogOnError] as string;

                if (!IsEmpty(ErrorText))
                {
                    HttpContext.Current.Session.Remove(KitSessionAccount.LogOnError);

                    return ErrorText;
                }

                return string.Empty;
            }
        }

        private bool IsAdministratorRequired
        {
            get
            {
                if (IsAssigned(SessionAccount))
                {
                    return IsAssigned(HttpContext.Current.Session[KitSessionAccount.AdministratorRequired]);
                }

                return false;
            }
        }

        public bool IsValidAccount
        {
            get
            {
                if (IsAdministratorRequired && !SessionAccount.IsAdministrator)
                {
                    return false;
                }

                return true;
            }
        }

        public bool IsAdministrator
        {
            get
            {
                if (IsAssigned(SessionAccount))
                {
                    return SessionAccount.IsAdministrator;
                }

                return false;
            }
        }
    }
}
