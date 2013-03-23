using Konfidence.Base;
using System.Web;
using Konfidence.BaseData;

namespace Konfidence.BaseUserControlHelpers.Login
{
    // - in de SessionAccount wordt bijgehouden wie is ingelogd en welke rechten de account heeft.
    // - in CurrentInternalAccount zit het accountobject dat in de applicatie is aangemaakt en gebruikt wordt om in te loggen
    class LoginContext : BaseItem
    {
        private InternalSessionAccount _AccountObject;
        private BaseDataItem _CurrentAccount;

        private InternalSessionAccount SessionAccount
        {
            get
            {
                if (IsAssigned(HttpContext.Current))
                {
                    return HttpContext.Current.Session[InternalSessionAccount.ACCOUNT_OBJECT] as InternalSessionAccount;
                }

                return _AccountObject;
            }
        }

        internal BaseDataItem CurrentInternalAccount
        {
            get
            {
                if (IsAssigned(HttpContext.Current))
                {
                    return HttpContext.Current.Session[InternalSessionAccount.CURRENT_ACCOUNT] as BaseDataItem;
                }

                return _CurrentAccount;
            }
            set
            {
                if (IsAssigned(HttpContext.Current))
                {
                    HttpContext.Current.Session[InternalSessionAccount.CURRENT_ACCOUNT] = value;
                }
                else
                {
                    _CurrentAccount = value;
                }
            }
        }

        internal bool IsLoggedIn
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

        internal bool IsAuthorized
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

        internal bool IsAdministrator
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

        private bool IsAdministratorRequired
        {
            get
            {
                if (IsAssigned(SessionAccount))
                {
                    return IsAssigned(HttpContext.Current.Session[InternalSessionAccount.ADMINISTRATOR_REQUIRED]);
                }

                return false;
            }
        }

        internal string LoginErrorMessage
        {
            get
            {
                var errorText = HttpContext.Current.Session[InternalSessionAccount.LOG_ON_ERROR] as string;

                if (!IsEmpty(errorText))
                {
                    HttpContext.Current.Session.Remove(InternalSessionAccount.LOG_ON_ERROR);

                    return errorText;
                }

                return string.Empty;
            }
        }

        internal string Email
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

        public LoginContext()
        {
            _CurrentAccount = null;
            _AccountObject = null;
        }

        internal void SessionLogon(string fullName, string email, string password, string loginPassword, bool isAdministrator)
        {
            if (!IsEmpty(password) && !IsEmpty(loginPassword))
            {
                if (password.Equals(loginPassword))
                {
                    HttpContext.Current.Session[InternalSessionAccount.ACCOUNT_OBJECT] = new InternalSessionAccount();

                    SessionAccount.FullName = fullName;
                    SessionAccount.Email = email;
                    SessionAccount.IsAdministrator = isAdministrator;

                    if (!IsAuthorized)
                    {
                        LogOff();
                    }
                }
            }
        }

        internal void LogOff()
        {
            if (IsAssigned(HttpContext.Current))
            {
                if (IsAssigned(HttpContext.Current.Session[InternalSessionAccount.ACCOUNT_OBJECT]))
                {
                    HttpContext.Current.Session.Remove(InternalSessionAccount.ACCOUNT_OBJECT);
                }

                if (IsAssigned(HttpContext.Current.Session[InternalSessionAccount.CURRENT_ACCOUNT]))
                {
                    HttpContext.Current.Session.Remove(InternalSessionAccount.CURRENT_ACCOUNT);
                }
            }
            else
            {
                _AccountObject = null;
                _CurrentAccount = null;
            }
        }
    }
}
