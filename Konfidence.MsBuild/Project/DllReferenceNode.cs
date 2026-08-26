using System.Xml;
using JetBrains.Annotations;
using Konfidence.MsBuild.ProjectBase;
using Konfidence.MsBuild.DocumentBase;

namespace Konfidence.MsBuild.Project
{
    public class DllReferenceNode : ProjectNode
    {
        private const string DllReferenceItemgroupName = "Reference";

        public DllReferenceNode(BaseProjectXmlDocument projectXmlDocument)
            : base(DllReferenceItemgroupName, projectXmlDocument)
        {
        }

        internal DllReferenceNode(BaseProjectXmlDocument projectXmlDocument, [NotNull] XmlNode itemGroupNode)
            : base(DllReferenceItemgroupName, projectXmlDocument, itemGroupNode, null)
        {
        }
    }
}
