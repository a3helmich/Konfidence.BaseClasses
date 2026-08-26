using System.Xml;
using JetBrains.Annotations;
using Konfidence.MsBuild.ProjectBase;

namespace Konfidence.MsBuild.Project
{
    public class ProjectCompileItemNode : ProjectItemNode
    {
        private readonly XmlNamespaceManager _xmlNamespaceManager;

        public ProjectCompileItemNode(XmlNode xmlNode, XmlNamespaceManager xmlNamespaceManager)
            : base(xmlNode)
        {
            _xmlNamespaceManager = xmlNamespaceManager;
        }

        [NotNull]
        public string FileName
        {
            get
            {
                if (projectXmlNode.Attributes != null)
                {
                    return projectXmlNode.Attributes["Include"].InnerText;
                }

                return string.Empty;
            }
        }
    }
}
