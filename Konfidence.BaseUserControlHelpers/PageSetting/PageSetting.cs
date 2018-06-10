using System;
using System.Xml;
using JetBrains.Annotations;
using Konfidence.Base;

namespace Konfidence.BaseUserControlHelpers.PageSetting
{
    public class PageSetting : BaseXmlDocument
    {
        public string PageName
        {
            get
            {
                GetAttributeValue(Root, "Name", out var value);

                return value;
            }
        }

        public string Role
        {
            get
            {
                GetValue("Role", out string role);

                return role;
            }
        }

        public bool IsLogonRequired => State.Equals("Authenticated", StringComparison.InvariantCultureIgnoreCase);

        public string State
        {
            get
            {
                GetValue("State", out string state);

                return state;
            }
        }

        public string SignInUrl
        {
            get
            {
                GetValue("SignInUrl", out string signInUrl);

                return signInUrl;
            }
        }

        [UsedImplicitly]
        public string HeaderText
        {
            get
            {
                GetValue("HeaderText", out string headerText);

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
