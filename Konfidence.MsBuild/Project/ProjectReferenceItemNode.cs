using System.Xml;
using Konfidence.MsBuild.ProjectBase;

namespace Konfidence.MsBuild.Project
{
    public class ProjectReferenceItemNode : ProjectItemNode
    {
        public ProjectReferenceItemNode(XmlNode xmlNode)
            : base(xmlNode)
        {
        }
    }
}
