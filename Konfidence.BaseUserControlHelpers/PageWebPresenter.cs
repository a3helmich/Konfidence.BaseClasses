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
using Konfidence.BaseUserControlHelpers.Login;
using System.IO;

namespace Konfidence.BaseUserControlHelpers
{
    public class BaseWebPresenter : BaseItem
    {
        private string _DataDirectory = string.Empty;
        private string _PageName = string.Empty;

        private PageSettingDictionary _PageSettingDictionary = null;
        private PageSettingXmlDocument _PageSettingDocument = null;

        private LoginContext _LoginContext = new LoginContext();

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
            get { return _LoginContext.CurrentInternalAccount; }
            set { _LoginContext.CurrentInternalAccount = value; }
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
            _LoginContext.SessionLogon(fullName, email, password, loginPassword, isAdministrator);
        }

        internal void SetPageName(string pageName)
        {
            _PageName = pageName;
        }

        public string ResolveServerPath(string url)
        {
            if (!IsEmpty(url))
            {
                return HttpContext.Current.Server.MapPath(url);
            }

            return string.Empty;
        }

        public virtual void LogOff()
        {
            _LoginContext.LogOff();
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
            get { return _LoginContext.Email; }
        }

        public bool IsLoggedIn
        {
            get { return _LoginContext.IsLoggedIn; }
        }

        public bool IsAuthorized
        {
            get { return _LoginContext.IsAuthorized; }
        }

        public string LoginErrorMessage
        {
            get { return _LoginContext.LoginErrorMessage; }
        }

        public bool IsAdministrator
        {
            get { return _LoginContext.IsAdministrator; }
        }

        protected PageSettingXmlDocument PageSettingDocument
        {
            get
            {
                if (!IsAssigned(_PageSettingDocument))
                {
                    _PageSettingDocument = new PageSettingXmlDocument();

                    CheckPageSettingFile();

                    _PageSettingDocument.Load(PageSettingFileName);
                }

                return _PageSettingDocument;
            }
        }

        private void CheckPageSettingFile()
        {
            if (!File.Exists(PageSettingFileName))
            {
                BaseXmlDocument pageSettings = new BaseXmlDocument();

                StringBuilder sb = new StringBuilder();

                sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                sb.AppendLine("<PageSetting>");
                sb.AppendLine("</PageSetting>");

                pageSettings.LoadXml(sb.ToString());

                pageSettings.Save(PageSettingFileName);
            }
        }

        private string PageSettingFileName
        {
            get
            {
                return DataDirectory + "PageSetting.nl.xml";
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

        public string SignInUrl
        {
            get
            {
                if (IsLoggedIn)
                {
                    if (PageSettingDictionary.ContainsKey(PageName))
                    {
                        return PageSettingDictionary[PageName].SignInUrl;
                    }
                }

                return string.Empty;
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

        private bool _IsLoaded;

        public bool IsLoaded
        {
            get { return _IsLoaded; }
            set { _IsLoaded = value; }
        }
    }
}
