﻿using System.Xml;
using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers.PageSetting
{
    public class PageSettingXmlDocument : BaseXmlDocument
    {
        private PageSettingDictionary _PageSettingDictionary;
        private string _MenuUrl = string.Empty;
        private string _LogonUrl = string.Empty;

        public string MenuUrl
        {
            get
            {
                if (PageSettingDictionary.Count > 0)
                {
                    return _MenuUrl;
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
                    return _LogonUrl;
                }

                return string.Empty;
            }
        }

        public PageSettingDictionary PageSettingDictionary
        {
            get
            {
                if (!_PageSettingDictionary.IsAssigned())
                {
                    _PageSettingDictionary = new PageSettingDictionary();

                    XmlNodeList pageSettingNodeList = Root.SelectNodes("Page");

                    if (pageSettingNodeList.IsAssigned())
                    {
                        foreach (XmlElement pageSettingElement in pageSettingNodeList)
                        {
                            var pageSetting = new PageSetting(pageSettingElement);

                            _PageSettingDictionary.Add(pageSetting.PageName.ToLowerInvariant(), pageSetting);

                            string role = pageSetting.Role.ToLowerInvariant();

                            switch (role)
                            {
                                case "login":
                                    _LogonUrl = pageSetting.PageName;
                                    break;
                                case "mainmenu":
                                    _MenuUrl = pageSetting.PageName;
                                    break;
                            }
                        }
                    }
                }

                return _PageSettingDictionary;
            }
        }
    }
}
