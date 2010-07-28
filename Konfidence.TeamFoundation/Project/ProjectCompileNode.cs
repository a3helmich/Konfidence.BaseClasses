using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.TeamFoundation.ProjectBase;
using System.Xml;

namespace Konfidence.TeamFoundation.Project
{
    public class ProjectCompileNode : ProjectNode
    {
        private const string PROJECT_COMPILE_ITEMGROUP_NAME = "Compile";

        public ProjectCompileNode(BaseTfsXmlDocument tfsXmlDocument)
            : base(PROJECT_COMPILE_ITEMGROUP_NAME, tfsXmlDocument)
        {
        }

        internal ProjectCompileNode(BaseTfsXmlDocument tfsXmlDocument, XmlNode itemGroupNode)
            : base(PROJECT_COMPILE_ITEMGROUP_NAME, tfsXmlDocument, itemGroupNode)
        {
        }
    }
}
