using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;
using System.Web;
using Konfidence.BaseData;

namespace Konfidence.BaseUserControlHelpers.Login
{
    // - in de SessionAccount wordt bijgehouden wie is ingelogd en welke rechten de account heeft.
    // - in CurrentInternalAccount zit het accountobject dat in de applicatie is aangemaakt en gebruikt wordt om in te loggen
    class LoginContext : BaseItem
    {
        private InternalSessionAccount _AccountObject = null;
        private BaseDataItem _CurrentAccount = null;

        private InternalSessionAccount SessionAccount
        {
            get
            {
                if (IsAssigned(HttpContext.Current))
                {
                    return HttpContext.Current.Session[InternalSessionAccount.AccountObject] as InternalSessionAccount;
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
                    return HttpContext.Current.Session[InternalSessionAccount.CurrentAccount] as BaseDataItem;
                }

                return _CurrentAccount;
            }
            set
            {
                if (IsAssigned(HttpContext.Current))
                {
                    HttpContext.Current.Session[InternalSessionAccount.CurrentAccount] = value;
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
                    return IsAssigned(HttpContext.Current.Session[InternalSessionAccount.AdministratorRequired]);
                }

                return false;
            }
        }

        internal string LoginErrorMessage
        {
            get
            {
                string ErrorText = HttpContext.Current.Session[InternalSessionAccount.LogOnError] as string;

                if (!IsEmpty(ErrorText))
                {
                    HttpContext.Current.Session.Remove(InternalSessionAccount.LogOnError);

                    return ErrorText;
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

        internal void SessionLogon(string fullName, string email, string password, string loginPassword, bool isAdministrator)
        {
            if (!IsEmpty(password) && !IsEmpty(loginPassword))
            {
                if (password.Equals(loginPassword))
                {
                    HttpContext.Current.Session[InternalSessionAccount.AccountObject] = new InternalSessionAccount();

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
                if (IsAssigned(HttpContext.Current.Session[InternalSessionAccount.AccountObject]))
                {
                    HttpContext.Current.Session.Remove(InternalSessionAccount.AccountObject);
                }

                if (IsAssigned(HttpContext.Current.Session[InternalSessionAccount.CurrentAccount]))
                {
                    HttpContext.Current.Session.Remove(InternalSessionAccount.CurrentAccount);
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
