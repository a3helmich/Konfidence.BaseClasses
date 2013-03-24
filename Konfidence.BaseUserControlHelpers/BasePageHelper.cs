using System;
using System.Web.UI;
using Konfidence.Base;


namespace Konfidence.BaseUserControlHelpers
{
    public class BasePageHelper: BaseItem
    {
        private readonly string[] _UrlParts = new string[0];
        private readonly string[] _RefererParts = new string[0];

        private string _CurrentDomainExtension = string.Empty;
        private string _CurrentLanguage = string.Empty;
        private string _CurrentDnsName = string.Empty;
        private string _CurrentPagePath = string.Empty;
        private string _CurrentPageName = string.Empty;

        private string _RefererDnsName = string.Empty;

        #region readonly properties

        public string CurrentDomainExtension
        {
            get
            {
                if (IsEmpty(_CurrentDomainExtension))
                {
                    _CurrentDomainExtension = GetCurrentDomainExtension();
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
                    _CurrentLanguage = GetCurrentLanguage();
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
                    _CurrentDnsName = GetCurrentDnsName();
                }

                return _CurrentDnsName;
            }
        }

        public string RefererDnsName
        {
            get
            {
                if (IsEmpty(_RefererDnsName))
                {
                    _RefererDnsName = GetRefererDnsName();
                }

                return _RefererDnsName;
            }
        }

        public string CurrentPagePath
        {
            get
            {
                if (IsEmpty(_CurrentPagePath))
                {
                    _CurrentPagePath = GetCurrentPagePath();
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
                    _CurrentPageName = GetCurrentPageName();
                }

                return _CurrentPageName;
            }
        }

        public bool IsValid
        {
            get { return Validate(); }
        }

        #endregion readonly  properties

        public BasePageHelper(string requestUrl, string refererUrl)
        {
            if (!IsEmpty(requestUrl))
            {
                _UrlParts = requestUrl.ToLowerInvariant().Split('/');
            }
            if (!IsEmpty(refererUrl))
            {
                _RefererParts = refererUrl.ToLowerInvariant().Split('/');
            }
        }

        private bool Validate()
        {
            if (_UrlParts.Length < 3)
            {
                SetErrorMessage("Url is too short to be true");

                return false;
            }

            if (_UrlParts[0] != "http:")
            {
                SetErrorMessage("Url does not start with http:");

                return false;
            }

            if (IsEmpty(CurrentDnsName))
            {
                SetErrorMessage("dns name is empty");

                return false;
            }

            if (!IsValidDnsName())
            {
                return false;
            }

            return true;
        }

        private bool IsValidDnsName()
        {
            var urlNodes = CurrentDnsName.Split('.');

            if (urlNodes.Length <= 1)
            {
                SetErrorMessage("dns name is not valid (1 node only)");
            }

            return true;
        }

        private string GetCurrentDomainExtension()
        {
            var currentDomainExtension = "nl"; // default

            var urlNodes = CurrentDnsName.Split('.');

            if (urlNodes.Length > 0)
            {
                currentDomainExtension = urlNodes[urlNodes.Length - 1];

                if (urlNodes.Length > 1)
                {
                    if (currentDomainExtension.Equals("uk"))
                    {
                        currentDomainExtension = urlNodes[urlNodes.Length - 2] + "." + urlNodes[urlNodes.Length - 1];
                    }
                }
            }

            return currentDomainExtension;
        }

        private string GetCurrentPagePath()
        {
            // strip the first three nodes from the string array
            var pagePathParts = new string[_UrlParts.Length - 3];

            Array.Copy(_UrlParts, 3, pagePathParts, 0, pagePathParts.Length);

            string currentPagePath = "/" + string.Join("/", pagePathParts);

            return currentPagePath;
        }

        private string GetCurrentPageName()
        {
            if (IsEmpty(CurrentPagePath))
            {
                return string.Empty;
            }

            string[] pagePathParts = CurrentPagePath.Split('/');

            return pagePathParts[pagePathParts.Length - 1]; // laatste element is altijd pageName
        }

        private string GetRefererDnsName()
        {
            string refererSite = "www.konfidence.nl";

            if (!_RefererParts[2].Equals("localhost"))
            {
                refererSite = _RefererParts[2];
            }

            return refererSite;
        }

        private string GetCurrentDnsName()
        {
            string currentSite = "www.konfidence.nl";

            if (!_UrlParts[2].Equals("localhost"))
            {
                currentSite = _UrlParts[2];
            }

            return currentSite;
        }

        private string GetCurrentLanguage()
        {
            string currentLanguage = "nl"; // default language

            if (!IsEmpty(CurrentDomainExtension))
            {
                string[] shortExtension = CurrentDomainExtension.Split('.'); // als uit meerdere nodes bestaat alleen de laatste oppikken

                string testExtension = shortExtension[shortExtension.Length - 1];

                switch (testExtension.ToLowerInvariant())
                {
                    case "nl":
                    case "be":
                        currentLanguage = "nl";
                        break;
                    case "de":
                        currentLanguage = "de";
                        break;
                    case "eu":
                    case "com":
                    case "org":
                    case "net":
                    case "uk":
                        currentLanguage = "uk";
                        break;
                    case "fr":
                        currentLanguage = "fr";
                        break;
                }
            }

            return currentLanguage;
        }

        internal static Control FindUserControlByType(ControlCollection controlCollection, Type findType)
        {
            foreach (Control x in controlCollection)
            {
                Type wantType = x.GetType();

                if (wantType.IsSubclassOf(findType))
                {
                    return x;
                }
            }

            return null;
        }
    }
}
