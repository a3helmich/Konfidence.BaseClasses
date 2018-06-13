using System;
using System.IO;
using System.Text;
using System.Web;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.BaseData;
using Konfidence.BaseUserControlHelpers.Login;
using Konfidence.BaseUserControlHelpers.PageSetting;

namespace Konfidence.BaseUserControlHelpers
{
    public class BaseWebPresenter 
    {
        private string _dataDirectory = string.Empty;
        private string _pageName = string.Empty;

        private PageSettingDictionary _pageSettingDictionary;
        private PageSettingXmlDocument _pageSettingDocument;

        private readonly LoginContext _loginContext = new LoginContext();

        public string PageName => _pageName;

        [UsedImplicitly]
        public string MenuPageName => PageSettingDocument.MenuUrl;

        [UsedImplicitly]
        public string MenuUrl
        {
            get
            {
                if (PageSettingDocument.MenuUrl.IsAssigned())
                {
                    return @"~\" + PageSettingDocument.MenuUrl;
                }

                return string.Empty;
            }
        }

        public string LogonPageName => PageSettingDocument.LogonUrl;

        public string LogonUrl
        {
            get
            {
                if (PageSettingDocument.LogonUrl.IsAssigned())
                {
                    return @"~\" + PageSettingDocument.LogonUrl;
                }

                return string.Empty;
            }
        }

        [UsedImplicitly]
        protected BaseDataItem CurrentInternalAccount
        {
            get => _loginContext.CurrentInternalAccount;
            set => _loginContext.CurrentInternalAccount = value;
        }

        [UsedImplicitly]
        public bool IsLocal => HttpContext.Current.Request.IsLocal;

        [UsedImplicitly]
        protected void SessionLogon(string fullName, string email, string password, string loginPassword, bool isAdministrator)
        {
            _loginContext.SessionLogon(fullName, email, password, loginPassword, isAdministrator);
        }

        internal void SetPageName(string pageName)
        {
            _pageName = pageName;
        }

        [UsedImplicitly]
        public virtual void LogOff()
        {
            _loginContext.LogOff();
        }

        /// <summary>
        /// returns App_Data path like 'c:\...\app_data' 
        /// </summary>
        public string DataDirectory
        {
            get
            {
                if (!_dataDirectory.IsAssigned())
                {
                    _dataDirectory = AppDomain.CurrentDomain.BaseDirectory;

                    if (AppDomain.CurrentDomain.GetData("DataDirectory").IsAssigned())
                    {
                        _dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
                    }

                    _dataDirectory += @"\";
                }

                return _dataDirectory;
            }
        }

        /// <summary>
        /// url to physical
        /// </summary>
        /// <param name="url">the url to resolve</param>
        /// <returns>the path like 'c:\...\foldername\filename' </returns>
        public static string ResolveServerPath(string url)
        {
            if (url.IsAssigned())
            {
                return HttpContext.Current.Server.MapPath(url);
            }

            return string.Empty;
        }

        /// <summary>
        /// physical to relative url
        /// </summary>
        /// <param name="serverPath">the path like 'c:\...\foldername'</param>
        /// <returns>the url '\foldername'</returns>
        [UsedImplicitly]
        public static string ResolveClientUrl(string serverPath)
        {
            return serverPath.Replace(ResolveServerPath(ApplicationUrl), @"~");
        }

        /// <summary>
        /// return the full path for the client
        /// </summary>
        /// <param name="pageUrl">url like \folder\page</param>
        /// <returns>http:\\...\folder\page</returns>
        [UsedImplicitly]
        public string ClientUrl(string pageUrl)
        {
            var appRoot = @"http://" + HttpContext.Current.Request.Url.Host;

            return appRoot + AbsolutePageUrl(pageUrl);
        }

        /// <summary>
        /// returns the application path like 'c:\...\foldername'
        /// </summary>
        [UsedImplicitly]
        public static string PhysicalApplicationPath => AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// return the application Url like '\applicationFoldername'
        /// </summary>
        public static string ApplicationUrl => VirtualPathUtility.ToAbsolute(@"~").TrimEnd('/');

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageUrl">Url like 'http:\\...\foldername'</param>
        /// <returns>Url like '~\foldername'</returns>
        [UsedImplicitly]
        public string RelativePageUrl(string pageUrl)
        {
            return VirtualPathUtility.ToAppRelative(pageUrl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageUrl">Url like '~\foldername'</param>
        /// <returns>Url like '\applicationFoldername\foldername'</returns>
        public string AbsolutePageUrl(string pageUrl)
        {
            return VirtualPathUtility.ToAbsolute(pageUrl);
        }

        [UsedImplicitly]
        public string PageUrl => HttpContext.Current.Request.Url.AbsolutePath;

        public string FromUrl
        {
            get => HttpContext.Current.Session[InternalSessionAccount.FROM_URL] as string;
            set => HttpContext.Current.Session[InternalSessionAccount.FROM_URL] = value;
        }

        [UsedImplicitly]
        public string Email => _loginContext.Email;

        public bool IsLoggedIn => _loginContext.IsLoggedIn;

        [UsedImplicitly]
        public bool IsAuthorized => _loginContext.IsAuthorized;

        [UsedImplicitly]
        public string LoginErrorMessage => _loginContext.LoginErrorMessage;

        [UsedImplicitly]
        public bool IsAdministrator => _loginContext.IsAdministrator;

        protected PageSettingXmlDocument PageSettingDocument
        {
            get
            {
                if (!_pageSettingDocument.IsAssigned())
                {
                    _pageSettingDocument = new PageSettingXmlDocument();

                    CheckPageSettingFile();

                    _pageSettingDocument.Load(PageSettingFileName);
                }

                return _pageSettingDocument;
            }
        }

        private void CheckPageSettingFile()
        {
            if (!File.Exists(PageSettingFileName))
            {
                var pageSettings = new BaseXmlDocument();

                var sb = new StringBuilder();

                sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                sb.AppendLine("<PageSetting>");
                sb.AppendLine("</PageSetting>");

                pageSettings.LoadXml(sb.ToString());

                pageSettings.Save(PageSettingFileName);
            }
        }

        private string PageSettingFileName => DataDirectory + "PageSetting.nl.xml";

        public PageSettingDictionary PageSettingDictionary
        {
            get
            {
                if (!_pageSettingDictionary.IsAssigned())
                {
                    _pageSettingDictionary = PageSettingDocument.PageSettingDictionary;
                }

                return _pageSettingDictionary;
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

        public bool IsLoaded { get; set; }
        public string ErrorMessage { get; private set; } = string.Empty;

        // TODO : convert to errorlist 
        public bool SetErrorMessage(string errorMessage)
        {
            ErrorMessage = errorMessage;

            return false;
        }

        [UsedImplicitly]
        public bool HasErrors()
        {
            return ErrorMessage.IsAssigned();
        }

        [UsedImplicitly]
        public void ClearErrorMessage()
        {
            ErrorMessage = string.Empty;
        }
    }
}
