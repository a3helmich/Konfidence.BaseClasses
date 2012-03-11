using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;
using System.Web.UI;
using System.Configuration;
using System.Web;
using Konfidence.BaseData;
using Konfidence.BaseUserControlHelpers.PageSetting;

namespace Konfidence.BaseUserControlHelpers
{
    public class BaseWebPresenter : BaseItem
    {
        private string _DataDirectory = string.Empty;
        private string _PageName = string.Empty;

        private PageSettingDictionary _PageSettingDictionary = null;
        private PageSettingXmlDocument _PageSettingDocument = null;

        public string PageName
        {
            get { return _PageName; }
        }

        public string MenuPageName
        {
            get
            {
                return PageSettingDocument.MenuUrl;
            }
        }

        public string MenuUrl
        {
            get
            {
                if (!IsEmpty(PageSettingDocument.MenuUrl))
                {
                    return @"~\" + PageSettingDocument.MenuUrl;
                }

                return string.Empty;
            }
        }

        public string LogonPageName
        {
            get
            {
                return PageSettingDocument.LogonUrl;
            }
        }

        public string LogonUrl
        {
            get
            {
                if (!IsEmpty(PageSettingDocument.LogonUrl))
                {
                    return @"~\" + PageSettingDocument.LogonUrl;
                }

                return string.Empty;
            }
        }

        protected BaseDataItem CurrentInternalAccount
        {
            get { return HttpContext.Current.Session[InternalSessionAccount.CurrentAccount] as BaseDataItem; }
            set { HttpContext.Current.Session[InternalSessionAccount.CurrentAccount] = value; }
        }

        private InternalSessionAccount SessionAccount
        {
            get { return HttpContext.Current.Session[InternalSessionAccount.AccountObject] as InternalSessionAccount; }
        }

        public bool IsLocal
        {
            get { return HttpContext.Current.Request.IsLocal; }
        }

        public string DataDirectory
        {
            get
            {
                if (IsEmpty(_DataDirectory))
                {
                    _DataDirectory = AppDomain.CurrentDomain.BaseDirectory;

                    if (AppDomain.CurrentDomain.GetData("DataDirectory") != null)
                    {
                        _DataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
                    }

                    _DataDirectory += @"\";
                }

                return _DataDirectory;
            }
        }

        protected void SessionLogon(string fullName, string email, string password, string loginPassword, bool isAdministrator)
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

        internal void SetPageName(string pageName)
        {
            _PageName = pageName;
        }

        public string ResolveClientUrl(string url)
        {
            if (!IsEmpty(url))
            {
                return HttpContext.Current.Server.MapPath(url);
            }

            return string.Empty;
        }

        public virtual void LogOff()
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

        public string ApplicationPath
        {
            get { return VirtualPathUtility.ToAbsolute(@"~").TrimEnd('/'); }
        }

        public string RelativePageUrl(string pageUrl)
        {
            return VirtualPathUtility.ToAppRelative(pageUrl);
        }

        public string AbsolutePageUrl(string pageUrl)
        {
            return VirtualPathUtility.ToAbsolute(pageUrl);
        }

        public string PageUrl
        {
            get { return HttpContext.Current.Request.Url.AbsolutePath; }
        }

        public string FromUrl
        {
            get { return HttpContext.Current.Session[InternalSessionAccount.FromUrl] as string; }
            set { HttpContext.Current.Session[InternalSessionAccount.FromUrl] = value; }
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
                string ErrorText = HttpContext.Current.Session[InternalSessionAccount.LogOnError] as string;

                if (!IsEmpty(ErrorText))
                {
                    HttpContext.Current.Session.Remove(InternalSessionAccount.LogOnError);

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
                    return IsAssigned(HttpContext.Current.Session[InternalSessionAccount.AdministratorRequired]);
                }

                return false;
            }
        }

        public bool IsAuthorized
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

        protected PageSettingXmlDocument PageSettingDocument
        {
            get
            {
                if (!IsAssigned(_PageSettingDocument))
                {
                    _PageSettingDocument = new PageSettingXmlDocument();

                    _PageSettingDocument.Load(DataDirectory + "PageSetting.nl.xml");
                }

                return _PageSettingDocument;
            }
        }

        public PageSettingDictionary PageSettingDictionary
        {
            get
            {
                if (!IsAssigned(_PageSettingDictionary))
                {
                    _PageSettingDictionary = PageSettingDocument.PageSettingDictionary;
                }

                return _PageSettingDictionary;
            }
        }
        public bool IsLogonRequired
        {
            get
            {
                if (PageSettingDictionary.ContainsKey(PageName))
                {
                    return PageSettingDictionary[PageName].IsLogonRequired;
                }

                return false;
            }
        }
    }
}
