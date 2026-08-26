using System.Xml;
using Konfidence.Base;

namespace Konfidence.MsBuild.ProjectBase
{
    // encapsulation van XmlNode
    public abstract class ProjectItemNode 
    {
        public XmlNode projectXmlNode { get; }

        protected ProjectItemNode(XmlNode xmlNode)
        {
            projectXmlNode = xmlNode;
        }
    }
}
