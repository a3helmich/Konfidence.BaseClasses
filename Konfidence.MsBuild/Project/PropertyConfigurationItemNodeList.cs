using System;
using System.Xml;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.MsBuild.ProjectBase;
using Konfidence.MsBuild.DocumentBase;

namespace Konfidence.MsBuild.Project
{
    class PropertyConfigurationItemNodeList : ProjectItemNodeList<PropertyConfigurationItemNode, PropertyConfigurationNode>
    {
        public PropertyConfigurationItemNodeList(BaseProjectXmlDocument projectXmlDocument) : base(projectXmlDocument)
        {
        }

        [NotNull]
        protected internal override PropertyConfigurationItemNode GetItemNode(XmlNode propertyConfigurationItemNode, XmlNamespaceManager xmlNamespaceManager)
        {
            return new PropertyConfigurationItemNode(propertyConfigurationItemNode);
        }

        [NotNull]
        protected internal override PropertyConfigurationNode GetGroupNode(BaseProjectXmlDocument projectXmlDocument)
        {
            return new PropertyConfigurationNode(projectXmlDocument);
        }

        protected internal override PropertyConfigurationNode CreateGroupNode(BaseProjectXmlDocument projectXmlDocument)
        {
            throw new NotImplementedException();
        }

        [NotNull]
        public string ProjectGuid
        {
            get
            {
                var element = GetElement("ProjectGuid");

                if (element.IsAssigned())
                {
                    return element.InnerText;
                }

                return string.Empty;
            }
            set
            {
                var element = GetElement("ProjectGuid");

                if (element.IsAssigned())
                {
                    if (value.IsGuid())
                    {
                        element.InnerText = value;
                    }
                }
            }
        }

        [CanBeNull]
        private XmlNode GetElement(string name)
        {
            foreach (var element in this)
            {
                if (element.projectXmlNode.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return element.projectXmlNode;
                }
            }

            return null;
        }
    }
}
