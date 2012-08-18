using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Konfidence.TeamFoundation.ProjectBase;

namespace Konfidence.TeamFoundation.Project
{
    public class ProjectNoneNode: ProjectNode
    {
        private const string PROJECT_NONE_ITEMGROUP_NAME = "None";

        public ProjectNoneNode(BaseTfsXmlDocument tfsXmlDocument)
            : base(PROJECT_NONE_ITEMGROUP_NAME, tfsXmlDocument)
        {
        }

        internal ProjectNoneNode(BaseTfsXmlDocument tfsXmlDocument, XmlNode itemGroupNode)
            : base(PROJECT_NONE_ITEMGROUP_NAME, tfsXmlDocument, itemGroupNode, null)
        {
        }
    }
}
