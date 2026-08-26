using System.Xml;
using JetBrains.Annotations;
using Konfidence.MsBuild.ProjectBase;
using Konfidence.MsBuild.DocumentBase;

namespace Konfidence.MsBuild.Project
{
    public class ProjectNoneNode: ProjectNode
    {
        private const string ProjectNoneItemgroupName = "None";

        public ProjectNoneNode(BaseProjectXmlDocument projectXmlDocument)
            : base(ProjectNoneItemgroupName, projectXmlDocument)
        {
        }

        internal ProjectNoneNode(BaseProjectXmlDocument projectXmlDocument, [NotNull] XmlNode itemGroupNode)
            : base(ProjectNoneItemgroupName, projectXmlDocument, itemGroupNode, null)
        {
        }
    }
}
