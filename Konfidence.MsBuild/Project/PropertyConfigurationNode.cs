using System.Xml;
using Konfidence.MsBuild.ProjectBase;
using Konfidence.MsBuild.DocumentBase;

namespace Konfidence.MsBuild.Project
{
    class PropertyConfigurationNode : ProjectNode
    {
        private const string ProjectConfigurationItemgroupName = "Configuration";

        public PropertyConfigurationNode(BaseProjectXmlDocument projectXmlDocument)
            : base(ProjectConfigurationItemgroupName, projectXmlDocument)
        {
        }

        internal PropertyConfigurationNode(BaseProjectXmlDocument projectXmlDocument, XmlNode itemGroupNode)
            : base(ProjectConfigurationItemgroupName, projectXmlDocument, null, itemGroupNode)
        {
        }
    }
}
