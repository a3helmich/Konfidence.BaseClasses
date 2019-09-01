using System.Xml;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers.PageSetting
{
    public class PageSettingXmlDocument : BaseXmlDocument
    {
        private PageSettingDictionary _pageSettingDictionary;
        private string _menuUrl = string.Empty;
        private string _logonUrl = string.Empty;

        public string MenuUrl
        {
            get
            {
                if (PageSettingDictionary.Count > 0)
                {
                    return _menuUrl;
                }

                return string.Empty;
            }
        }

        public string LogonUrl
        {
            get
            {
                if (PageSettingDictionary.Count > 0)
                {
                    return _logonUrl;
                }

                return string.Empty;
            }
        }

        [NotNull]
        public PageSettingDictionary PageSettingDictionary
        {
            get
            {
                if (!_pageSettingDictionary.IsAssigned())
                {
                    _pageSettingDictionary = new PageSettingDictionary();

                    var pageSettingNodeList = Root.SelectNodes("Page");

                    if (pageSettingNodeList.IsAssigned())
                    {
                        foreach (XmlElement pageSettingElement in pageSettingNodeList)
                        {
                            var pageSetting = new PageSetting(pageSettingElement);

                            _pageSettingDictionary.Add(pageSetting.PageName.ToLowerInvariant(), pageSetting);

                            var role = pageSetting.Role.ToLowerInvariant();

                            switch (role)
                            {
                                case "login":
                                    _logonUrl = pageSetting.PageName;
                                    break;
                                case "mainmenu":
                                    _menuUrl = pageSetting.PageName;
                                    break;
                            }
                        }
                    }
                }

                return _pageSettingDictionary;
            }
        }
    }
}
