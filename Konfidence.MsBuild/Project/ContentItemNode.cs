using System.Xml;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.MsBuild.ProjectBase;

namespace Konfidence.MsBuild.Project
{
    public class ContentItemNode : ProjectItemNode
    {
        private readonly XmlNamespaceManager _xmlNamespaceManager;

        #region readOnlyProperties
        [NotNull]
        public string Include
        {
            get
            {
                if (projectXmlNode.IsAssigned() && projectXmlNode.Attributes.IsAssigned() && projectXmlNode.Attributes["Include"].IsAssigned())
                {
                    return projectXmlNode.Attributes["Include"].InnerText;
                }

                return string.Empty;
            }
        }
        #endregion readOnlyProperties

        public ContentItemNode(XmlNode xmlNode, XmlNamespaceManager xmlNamespaceManager) : base(xmlNode)
        {
            _xmlNamespaceManager = xmlNamespaceManager;
        }
    }
}
