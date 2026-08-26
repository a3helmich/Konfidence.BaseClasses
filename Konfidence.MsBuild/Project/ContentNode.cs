using System.Xml;
using JetBrains.Annotations;
using Konfidence.MsBuild.ProjectBase;
using Konfidence.MsBuild.DocumentBase;

namespace Konfidence.MsBuild.Project
{
    public class ContentNode : ProjectNode
    {
        private const string DllContentItemgroupName = "Content";

        
        public ContentNode(BaseProjectXmlDocument projectXmlDocument) : base(DllContentItemgroupName, projectXmlDocument)
        {
        }

        internal ContentNode(BaseProjectXmlDocument projectXmlDocument, [NotNull] XmlNode itemGroupNode) : base(DllContentItemgroupName, projectXmlDocument, itemGroupNode, null)
        {
        }

    }
}
