using System.Xml;
using JetBrains.Annotations;
using Konfidence.MsBuild.ProjectBase;
using Konfidence.MsBuild.DocumentBase;

namespace Konfidence.MsBuild.Project
{
    public class ProjectCompileNode : ProjectNode
    {
        private const string ProjectCompileItemgroupName = "Compile";

        public ProjectCompileNode(BaseProjectXmlDocument projectXmlDocument)
            : base(ProjectCompileItemgroupName, projectXmlDocument)
        {
        }

        internal ProjectCompileNode(BaseProjectXmlDocument projectXmlDocument, [NotNull] XmlNode itemGroupNode)
            : base(ProjectCompileItemgroupName, projectXmlDocument, itemGroupNode, null)
        {
        }
    }
}
