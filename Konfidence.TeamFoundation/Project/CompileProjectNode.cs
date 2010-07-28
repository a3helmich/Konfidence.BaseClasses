using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.TeamFoundation.ProjectBase;
using System.Xml;

namespace Konfidence.TeamFoundation.Project
{
    public class CompileProjectNode : ProjectNode
    {
        private const string PROJECT_COMPILE_ITEMGROUP_NAME = "Compile";

        public CompileProjectNode(BaseTfsXmlDocument tfsXmlDocument)
            : base(PROJECT_COMPILE_ITEMGROUP_NAME, tfsXmlDocument)
        {
        }

        internal CompileProjectNode(BaseTfsXmlDocument tfsXmlDocument, XmlNode itemGroupNode)
            : base(PROJECT_COMPILE_ITEMGROUP_NAME, tfsXmlDocument, itemGroupNode)
        {
        }
    }
}
