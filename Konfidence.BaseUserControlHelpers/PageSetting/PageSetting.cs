﻿using System;
using System.Xml;
using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers.PageSetting
{
    public class PageSetting : BaseXmlDocument
    {
        public string PageName
        {
            get
            {
                string value;

                GetAttributeValue(Root, "Name", out value);

                return value;
            }
        }

        public string Role
        {
            get
            {
                string role;

                GetValue("Role", out role);

                return role;
            }
        }

        public bool IsLogonRequired
        {
            get
            {
                if (State.Equals("Authenticated", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }

                return false;
            }
        }

        public string State
        {
            get
            {
                string state;

                GetValue("State", out state);

                return state;
            }
        }

        public string SignInUrl
        {
            get
            {
                string signInUrl;

                GetValue("SignInUrl", out signInUrl);

                return signInUrl;
            }
        }

        public string HeaderText
        {
            get
            {
                string headerText;

                GetValue("HeaderText", out headerText);

                return headerText;
            }
        }

        public PageSetting(XmlElement pageSettingElement)
        {
            LoadXml(pageSettingElement.OuterXml);
        }

        public sealed override void LoadXml(string xml)
        {
            base.LoadXml(xml);
        }
    }
}
