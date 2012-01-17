using System;
using System.Web;
using System.Web.UI;
using Konfidence.Base;
using System.Globalization;


namespace Konfidence.BaseWebsiteClasses
{
    internal class BasePageHelper: BaseItem
    {
        private string[] _UrlParts = new string[0];

        private string _CurrentDomainExtension = string.Empty;
        private string _CurrentLanguage = string.Empty;
        private string _CurrentDnsName = string.Empty;
        private string _CurrentPagePath = string.Empty;
        private string _CurrentPageName = string.Empty;

        #region readonly Session properties
        public string CurrentDomainExtension
        {
            get
            {
                if (IsEmpty(_CurrentDomainExtension))
                {
                    _CurrentDomainExtension = GetCurrentDomainExtension(_UrlParts);
                }

                return _CurrentDomainExtension;
            }
        }

        public string CurrentLanguage
        {
            get
            {
                if (IsEmpty(_CurrentLanguage))
                {
                    _CurrentLanguage = GetCurrentLanguage(_UrlParts);
                }

                return _CurrentLanguage;
            }
        }

        public string CurrentDnsName
        {
            get
            {
                if (IsEmpty(_CurrentDnsName))
                {
                    _CurrentDnsName = GetCurrentDnsName(_UrlParts);
                }

                return _CurrentDnsName;
            }
        }

        public string CurrentPagePath
        {
            get
            {
                if (IsEmpty(_CurrentPagePath))
                {
                    _CurrentPagePath = GetCurrentPagePath(_UrlParts);
                }

                return _CurrentPagePath;
            }
        }

        public string CurrentPageName
        {
            get
            {
                if (IsEmpty(_CurrentPageName))
                {
                    _CurrentPageName = GetCurrentPageName(_UrlParts);
                }

                return _CurrentPageName;
            }
        }
        #endregion readonly Session properties

        public BasePageHelper(string requestUrl)
        {
            if (!IsEmpty(requestUrl))
            {
                _UrlParts = requestUrl.ToLowerInvariant().Split('/');
            }
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

        private static string GetCurrentPageName(string[] urlParts)
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

        private static string GetCurrentDnsName(string[] urlParts)
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
