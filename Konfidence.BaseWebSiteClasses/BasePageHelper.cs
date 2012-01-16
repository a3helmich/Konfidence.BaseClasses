using System;
using System.Web;
using System.Web.UI;
using Konfidence.Base;
using System.Globalization;


namespace Konfidence.BaseWebsiteClasses
{
    internal class BasePageHelper: BaseItem
    {
        private Page _CurrentPage;

        private const string CURRENT_DOMAIN_EXTENSION = "CurrentDomainExtension";
        private const string CURRENT_LANGUAGE = "CurrentLanguage";
        private const string CURRENT_DNS_NAME = "CurrentSite";
        private const string CURRENT_PAGE_PATH = "CurrentPagePath";
        private const string CURRENT_PAGE_NAME = "CurrentPageName";

        #region readonly Session properties
        public string CurrentDomainExtension
        {
            get
            {
                string newCurrentDomainExtension = string.Empty;

                if (!IsEmpty(_CurrentPage.Session[CURRENT_DOMAIN_EXTENSION] as string))
                {
                    newCurrentDomainExtension = _CurrentPage.Session[CURRENT_DOMAIN_EXTENSION] as string;
                }

                return newCurrentDomainExtension;
            }
        }

        public string CurrentLanguage
        {
            get
            {
                string newCurrentLanguage = string.Empty;

                if (!IsEmpty(_CurrentPage.Session[CURRENT_LANGUAGE] as string))
                {
                    newCurrentLanguage = _CurrentPage.Session[CURRENT_LANGUAGE] as string;
                }

                return newCurrentLanguage;
            }
        }

        public string CurrentDnsName
        {
            get
            {
                string newCurrentDnsName = string.Empty;

                if (!IsEmpty(_CurrentPage.Session[CURRENT_DNS_NAME] as string))
                {
                    newCurrentDnsName = _CurrentPage.Session[CURRENT_DNS_NAME] as string;
                }

                return newCurrentDnsName;
            }
        }

        public string CurrentPagePath
        {
            get
            {
                string newCurrentPagePath = string.Empty;

                if (!IsEmpty(_CurrentPage.Session[CURRENT_PAGE_PATH] as string))
                {
                    newCurrentPagePath = _CurrentPage.Session[CURRENT_PAGE_PATH] as string;
                }

                return newCurrentPagePath;
            }
        }

        public string CurrentPageName
        {
            get
            {
                string newCurrentPageName = string.Empty;

                if (!IsEmpty(_CurrentPage.Session[CURRENT_PAGE_NAME] as string))
                {
                    newCurrentPageName = _CurrentPage.Session[CURRENT_PAGE_NAME] as string;
                }

                return newCurrentPageName;
            }
        }
        #endregion readonly Session properties

        public BasePageHelper(MasterPage masterPage) // alleen van de sessionvariabelen gebruik maken 
        {
            _CurrentPage = masterPage.Page;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public BasePageHelper(UserControl userControl) // alleen van de sessionvariabelen gebruik maken 
        {
            _CurrentPage = userControl.Page;
        }

        public BasePageHelper(Page currentPage) // zet ook alle sessionvariabelen
        {
            _CurrentPage = currentPage;

            if (!_CurrentPage.IsPostBack)
            {
                SetSessionProperties();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        private string[] urlParts
        {
            get
            {
                return _CurrentPage.Request.Url.ToString().ToLowerInvariant().Split('/');
            }
        }

        private void SetSessionProperties()
        {
            _CurrentPage.Session[CURRENT_LANGUAGE] = GetCurrentLanguage(urlParts);

            _CurrentPage.Session[CURRENT_DNS_NAME] = GetCurrentSite(urlParts);

            _CurrentPage.Session[CURRENT_PAGE_PATH] = GetCurrentPagePath(urlParts);

            _CurrentPage.Session[CURRENT_PAGE_NAME] = GetCurrentPageName(urlParts);

            _CurrentPage.Session[CURRENT_DOMAIN_EXTENSION] = GetCurrentDomainExtension(urlParts);
        }

        private static string GetCurrentDomainExtension(string[] urlParts)
        {
            string currentDomainExtension = "nl";

            if (urlParts[0].Equals("http:"))
            {
                string[] UrlNodes = urlParts[2].Split('.');

                if (UrlNodes.Length > 0)
                {
                    string firstNode = UrlNodes[0];
                    string lastNode = UrlNodes[UrlNodes.Length - 1];

                    currentDomainExtension = lastNode;

                    if (lastNode.Equals("uk") || firstNode.Equals("wwwuk"))
                    {
                        currentDomainExtension = "co.uk";
                    }
                }
            }

            return currentDomainExtension;
        }

        private static string GetCurrentPagePath(string[] urlParts)
        {
            string currentPagePath = string.Empty;

            if (urlParts[0].Equals("http:"))
            {
                string[] pagePathParts = new string[urlParts.Length - 3];

                Array.Copy(urlParts, 3, pagePathParts, 0, pagePathParts.Length);

                currentPagePath = "/" + string.Join("/", pagePathParts);
            }

            return currentPagePath;
        }

        private static object GetCurrentPageName(string[] urlParts)
        {
            string currentPageName = string.Empty;

            if (urlParts[0].Equals("http:"))
            {
                string[] pagePathParts = new string[urlParts.Length - 3];

                Array.Copy(urlParts, 3, pagePathParts, 0, pagePathParts.Length);

                currentPageName = pagePathParts[1];
            }

            return currentPageName;
        }

        private static string GetCurrentSite(string[] urlParts)
        {
            string currentSite = "www.konfidence.nl";

            if (urlParts[0].Equals("http:"))
            {
                if (!urlParts[2].Equals("localhost"))
                {
                    currentSite = urlParts[2];
                }
            }

            return currentSite;
        }

        private static string GetCurrentLanguage(string[] urlParts)
        {
            string currentLanguage = "nl"; // default language

            if (urlParts[0].Equals("http:"))
            {
                string[] UrlNodes = urlParts[2].Split('.');

                if (UrlNodes.Length > 0)
                {
                    string firstNode = UrlNodes[0];
                    string lastNode = UrlNodes[UrlNodes.Length - 1];

                    switch (lastNode)
                    {
                        case "nl":
                        case "be":
                            currentLanguage = "nl";
                            break;
                        case "DE":
                            currentLanguage = "de";
                            break;
                        case "eu":
                        case "com":
                        case "uk":
                            currentLanguage = "uk";
                            break;
                        case "fr":
                            currentLanguage = "fr";
                            break;
                        case "ru":
                            currentLanguage = "ru";
                            break;
                    }

                    if (firstNode.Equals("wwwuk"))
                    {
                        currentLanguage = "uk";
                    }
                }
            }

            return currentLanguage;
        }
    }
}
