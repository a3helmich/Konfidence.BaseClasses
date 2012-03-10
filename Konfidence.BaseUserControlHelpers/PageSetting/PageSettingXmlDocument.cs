using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;
using System.Xml;

namespace Konfidence.BaseUserControlHelpers.PageSetting
{
    public class PageSettingXmlDocument : BaseXmlDocument
    {
        private PageSettingDictionary _PageSettingDictionary = null;
        private string _MenuPage = string.Empty;
        private string _LogonPage = string.Empty;

        public string MenuPage
        {
            get
            {
                if (PageSettingDictionary.Count > 0)
                {
                    return _MenuPage;
                }

                return string.Empty;
            }
        }

        public string LogonPage
        {
            get
            {
                if (PageSettingDictionary.Count > 0)
                {
                    return _LogonPage;
                }

                return string.Empty;
            }
        }

        public PageSettingDictionary PageSettingDictionary
        {
            get
            {
                if (!IsAssigned(_PageSettingDictionary))
                {
                    _PageSettingDictionary = new PageSettingDictionary();

                    XmlNodeList pageSettingNodeList = Root.SelectNodes("Page");

                    if (IsAssigned(pageSettingNodeList))
                    {
                        foreach (XmlElement pageSettingElement in pageSettingNodeList)
                        {
                            PageSetting pageSetting = new PageSetting(pageSettingElement);

                            _PageSettingDictionary.Add(pageSetting.PageName.ToLowerInvariant(), pageSetting);

                            string role = pageSetting.Role.ToLowerInvariant();

                            switch (role)
                            {
                                case "login":
                                    _LogonPage = pageSetting.PageName;
                                    break;
                                case "mainmenu":
                                    _MenuPage = pageSetting.PageName;
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
