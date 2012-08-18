using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.TeamFoundation.ProjectBase;
using System.Xml;

namespace Konfidence.TeamFoundation.Project
{
    class PropertyConfigurationNode : ProjectNode
    {
        private const string PROJECT_CONFIGURATION_ITEMGROUP_NAME = "Configuration";

        public PropertyConfigurationNode(BaseTfsXmlDocument tfsXmlDocument)
            : base(PROJECT_CONFIGURATION_ITEMGROUP_NAME, tfsXmlDocument)
        {
        }

        internal PropertyConfigurationNode(BaseTfsXmlDocument tfsXmlDocument, XmlNode itemGroupNode)
            : base(PROJECT_CONFIGURATION_ITEMGROUP_NAME, tfsXmlDocument, itemGroupNode)
        {
        }
    }
}
