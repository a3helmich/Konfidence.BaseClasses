using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.TeamFoundation.ProjectBase;
using System.Xml;

namespace Konfidence.TeamFoundation.Project
{
    class PropertyConfigurationItemNodeList : ProjectItemNodeList<PropertyConfigurationItemNode, PropertyConfigurationNode>
    {
        private BaseTfsXmlDocument _TfsXmlDocument = null;

        public PropertyConfigurationItemNodeList(BaseTfsXmlDocument tfsXmlDocument) : base(tfsXmlDocument)
        {
            _TfsXmlDocument = tfsXmlDocument;
        }

        protected internal override PropertyConfigurationItemNode GetItemNode(XmlNode propertyConfigurationItemNode, XmlNamespaceManager xmlNamespaceManager)
        {
            return new PropertyConfigurationItemNode(propertyConfigurationItemNode, xmlNamespaceManager);
        }

        protected internal override PropertyConfigurationNode GetGroupNode(BaseTfsXmlDocument tfsXmlDocument)
        {
            return new PropertyConfigurationNode(tfsXmlDocument);
        }

        protected internal override PropertyConfigurationNode CreateGroupNode(BaseTfsXmlDocument tfsXmlDocument)
        {
            throw new NotImplementedException();
        }

        public string ProjectGuid
        {
            get
            {
                XmlNode element = this.GetElement("ProjectGuid");

                if (IsAssigned(element))
                {
                    return element.InnerText;
                }

                return string.Empty;
            }
        }

        private XmlNode GetElement(string name)
        {
            foreach (PropertyConfigurationItemNode element in this)
            {
                if (element.TfsXmlNode.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return element.TfsXmlNode;
                }
            }

            return null;
        }
    }
}
