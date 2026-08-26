using System.Xml;
using JetBrains.Annotations;
using Konfidence.MsBuild.ProjectBase;

namespace Konfidence.MsBuild.Project
{
    public class ProjectNoneItemNode: ProjectItemNode
    {
        //private XmlNamespaceManager _XmlNamespaceManager;

        public ProjectNoneItemNode(XmlNode xmlNode) : base(xmlNode)
        {
            //_XmlNamespaceManager = xmlNamespaceManager;
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
