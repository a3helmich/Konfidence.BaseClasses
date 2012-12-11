﻿using System;
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

        protected void SessionLogon(string fullName, string email, string password, string loginPassword, bool isAdministrator)
        {
            _LoginContext.SessionLogon(fullName, email, password, loginPassword, isAdministrator);
        }

        internal void SetPageName(string pageName)
        {
            _PageName = pageName;
        }

        public virtual void LogOff()
        {
            _LoginContext.LogOff();
        }

        /// <summary>
        /// returns App_Data path like 'c:\...\app_data' 
        /// </summary>
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

        /// <summary>
        /// url to physical
        /// </summary>
        /// <param name="url">the url to resolve</param>
        /// <returns>the path like 'c:\...\foldername\filename' </returns>
        public static string ResolveServerPath(string url)
        {
            if (!IsEmpty(url))
            {
                return HttpContext.Current.Server.MapPath(url);
            }

            return string.Empty;
        }

        /// <summary>
        /// physical to relative url
        /// </summary>
        /// <param name="serverPath">the path like 'c:\...\foldername'</param>
        /// <returns>the '\url'</returns>
        public static string ResolveClientUrl(string serverPath)
        {
            return serverPath.Replace(ResolveServerPath(ApplicationUrl), @"~");
        }

        /// <summary>
        /// returns the application path like 'c:\...\foldername'
        /// </summary>
        public static string PhysicalApplicationPath
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        /// <summary>
        /// return the application Url like '\applicationFoldername'
        /// </summary>
        public static string ApplicationUrl
        {
            get { return VirtualPathUtility.ToAbsolute(@"~").TrimEnd('/'); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageUrl">Url like 'http:\\...\foldername'</param>
        /// <returns>Url like '~\foldername'</returns>
        public string RelativePageUrl(string pageUrl)
        {
            return VirtualPathUtility.ToAppRelative(pageUrl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageUrl">Url like 'http:\\...\foldername'</param>
        /// <returns>Url like '\applicationFoldername\foldername'</returns>
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
