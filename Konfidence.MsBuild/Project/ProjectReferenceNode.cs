using System.Xml;
using JetBrains.Annotations;
using Konfidence.MsBuild.ProjectBase;
using Konfidence.MsBuild.DocumentBase;

namespace Konfidence.MsBuild.Project
{
    public class ProjectReferenceNode : ProjectNode
    {
        private const string ProjectReferenceItemgroupName = "ProjectReference";

        public ProjectReferenceNode(BaseProjectXmlDocument projectXmlDocument)
            : base(ProjectReferenceItemgroupName, projectXmlDocument)
        {
        }

        internal ProjectReferenceNode(BaseProjectXmlDocument projectXmlDocument, [NotNull] XmlNode itemGroupNode)
            : base(ProjectReferenceItemgroupName, projectXmlDocument, itemGroupNode, null)
        {
        }
    }
}
