using System;
using System.Web.UI;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers
{
    public class BasePageHelper
    {
        private readonly string[] _urlParts = new string[0];
        private readonly string[] _refererParts = new string[0];

        private string _currentDomainExtension = string.Empty;
        private string _currentLanguage = string.Empty;
        private string _currentDnsName = string.Empty;
        private string _currentPagePath = string.Empty;
        private string _currentPageName = string.Empty;

        private string _refererDnsName = string.Empty;

        #region readonly properties

        [NotNull]
        public string CurrentDomainExtension
        {
            get
            {
                if (!_currentDomainExtension.IsAssigned())
                {
                    _currentDomainExtension = GetCurrentDomainExtension();
                }

                return _currentDomainExtension;
            }
        }

        [NotNull]
        public string CurrentLanguage
        {
            get
            {
                if (!_currentLanguage.IsAssigned())
                {
                    _currentLanguage = GetCurrentLanguage();
                }

                return _currentLanguage;
            }
        }

        [NotNull]
        public string CurrentDnsName
        {
            get
            {
                if (!_currentDnsName.IsAssigned())
                {
                    _currentDnsName = GetCurrentDnsName();
                }

                return _currentDnsName;
            }
        }

        [NotNull]
        public string RefererDnsName
        {
            get
            {
                if (!_refererDnsName.IsAssigned())
                {
                    _refererDnsName = GetRefererDnsName();
                }

                return _refererDnsName;
            }
        }

        [NotNull]
        public string CurrentPagePath
        {
            get
            {
                if (!_currentPagePath.IsAssigned())
                {
                    _currentPagePath = GetCurrentPagePath();
                }

                return _currentPagePath;
            }
        }

        [NotNull]
        public string CurrentPageName
        {
            get
            {
                if (!_currentPageName.IsAssigned())
                {
                    _currentPageName = GetCurrentPageName();
                }

                return _currentPageName;
            }
        }

        public bool IsValid => Validate();

        #endregion readonly  properties

        public BasePageHelper(string requestUrl, string refererUrl)
        {
            if (requestUrl.IsAssigned())
            {
                _urlParts = requestUrl.ToLowerInvariant().Split('/');
            }
            if (refererUrl.IsAssigned())
            {
                _refererParts = refererUrl.ToLowerInvariant().Split('/');
            }
        }

        private bool Validate()
        {
            if (_urlParts.Length < 3)
            {
                SetErrorMessage("Url is too short to be true");

                return false;
            }

            if (_urlParts[0] != "http:")
            {
                SetErrorMessage("Url does not start with http:");

                return false;
            }

            if (!CurrentDnsName.IsAssigned())
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

        [NotNull]
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

        [NotNull]
        private string GetCurrentPagePath()
        {
            // strip the first three nodes from the string array
            var pagePathParts = new string[_urlParts.Length - 3];

            Array.Copy(_urlParts, 3, pagePathParts, 0, pagePathParts.Length);

            var currentPagePath = "/" + string.Join("/", pagePathParts);

            return currentPagePath;
        }

        [NotNull]
        private string GetCurrentPageName()
        {
            if (!CurrentPagePath.IsAssigned())
            {
                return string.Empty;
            }

            var pagePathParts = CurrentPagePath.Split('/');

            return pagePathParts[pagePathParts.Length - 1]; // laatste element is altijd pageName
        }

        [NotNull]
        private string GetRefererDnsName()
        {
            var refererSite = "www.konfidence.nl";

            if (!_refererParts[2].Equals("localhost"))
            {
                refererSite = _refererParts[2];
            }

            return refererSite;
        }

        [NotNull]
        private string GetCurrentDnsName()
        {
            var currentSite = "www.konfidence.nl";

            if (!_urlParts[2].Equals("localhost"))
            {
                currentSite = _urlParts[2];
            }

            return currentSite;
        }

        [NotNull]
        private string GetCurrentLanguage()
        {
            var currentLanguage = "nl"; // default language

            if (CurrentDomainExtension.IsAssigned())
            {
                var shortExtension = CurrentDomainExtension.Split('.'); // als uit meerdere nodes bestaat alleen de laatste oppikken

                var testExtension = shortExtension[shortExtension.Length - 1];

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

        [CanBeNull]
        internal static Control FindUserControlByType([NotNull] ControlCollection controlCollection, Type findType)
        {
            foreach (Control x in controlCollection)
            {
                var wantType = x.GetType();

                if (wantType.IsSubclassOf(findType))
                {
                    return x;
                }
            }

            return null;
        }
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
