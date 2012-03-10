using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;
using System.Xml;

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

        public string State
        {
            get
            {
                string state;

                GetValue("State", out state);

                return state;
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
    }
}
