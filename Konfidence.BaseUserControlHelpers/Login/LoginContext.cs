using System.Web;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.BaseData;

namespace Konfidence.BaseUserControlHelpers.Login
{
    // - in de SessionAccount wordt bijgehouden wie is ingelogd en welke rechten de account heeft.
    // - in CurrentInternalAccount zit het accountobject dat in de applicatie is aangemaakt en gebruikt wordt om in te loggen
    internal class LoginContext 
    {
        private InternalSessionAccount _accountObject;
        private BaseDataItem _currentAccount;

        private InternalSessionAccount SessionAccount
        {
            get
            {
                if (HttpContext.Current.IsAssigned())
                {
                    return HttpContext.Current.Session[InternalSessionAccount.ACCOUNT_OBJECT] as InternalSessionAccount;
                }

                return _accountObject;
            }
        }

        internal BaseDataItem CurrentInternalAccount
        {
            get
            {
                if (HttpContext.Current.IsAssigned())
                {
                    return HttpContext.Current.Session[InternalSessionAccount.CURRENT_ACCOUNT] as BaseDataItem;
                }

                return _currentAccount;
            }
            set
            {
                if (HttpContext.Current.IsAssigned())
                {
                    HttpContext.Current.Session[InternalSessionAccount.CURRENT_ACCOUNT] = value;
                }
                else
                {
                    _currentAccount = value;
                }
            }
        }

        internal bool IsLoggedIn
        {
            get
            {
                if (SessionAccount.IsAssigned())
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
                if (SessionAccount.IsAssigned())
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
                if (SessionAccount.IsAssigned())
                {
                    return HttpContext.Current.Session[InternalSessionAccount.ADMINISTRATOR_REQUIRED].IsAssigned();
                }

                return false;
            }
        }

        [NotNull]
        internal string LoginErrorMessage
        {
            get
            {
                var errorText = HttpContext.Current.Session[InternalSessionAccount.LOG_ON_ERROR] as string;

                if (errorText.IsAssigned())
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
                if (SessionAccount.IsAssigned())
                {
                    return SessionAccount.Email;
                }

                return string.Empty;
            }
        }

        public LoginContext()
        {
            _currentAccount = null;
            _accountObject = null;
        }

        internal void SessionLogon(string fullName, string email, string password, string loginPassword, bool isAdministrator)
        {
            if (password.IsAssigned() && loginPassword.IsAssigned())
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
            if (HttpContext.Current.IsAssigned())
            {
                if (HttpContext.Current.Session[InternalSessionAccount.ACCOUNT_OBJECT].IsAssigned())
                {
                    HttpContext.Current.Session.Remove(InternalSessionAccount.ACCOUNT_OBJECT);
                }

                if (HttpContext.Current.Session[InternalSessionAccount.CURRENT_ACCOUNT].IsAssigned())
                {
                    HttpContext.Current.Session.Remove(InternalSessionAccount.CURRENT_ACCOUNT);
                }
            }
            else
            {
                _accountObject = null;
                _currentAccount = null;
            }
        }
    }
}
