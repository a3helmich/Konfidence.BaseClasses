﻿using System;
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

        public string MenuPage
        {
            get { return ConfigurationManager.AppSettings["MenuPage"] as string; }
        }

        public SessionAccount SessionAccount
        {
            get
            {
                return HttpContext.Current.Session[KitSessionAccount.AccountObject] as SessionAccount;
            }
        }

        public bool IsLocal
        {
            get
            {
                return HttpContext.Current.Request.IsLocal;
            }
        }

        public virtual void LogOff()
        {
            if (IsAssigned(HttpContext.Current.Session[KitSessionAccount.AccountObject]))
            {
                HttpContext.Current.Session.Remove(KitSessionAccount.AccountObject);
            }
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
