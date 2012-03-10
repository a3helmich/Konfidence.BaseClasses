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
