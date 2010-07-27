using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Konfidence.TeamFoundation.ProjectBase;

namespace Konfidence.TeamFoundation.Project
{
    public class ProjectReferenceItemNodeList : ProjectItemNodeList<ProjectReferenceItemNode, ProjectReferenceNode>
    {
        public ProjectReferenceItemNodeList(BaseTfsXmlDocument tfsXmlDocument)
            : base(tfsXmlDocument)
        {
        }

        // wordt alleen ge-called vanuit het base object
        internal protected override ProjectReferenceItemNode GetItemNode(XmlNode projectItemNode, XmlNamespaceManager xmlNamespaceManager)
        {
            return new ProjectReferenceItemNode(projectItemNode, xmlNamespaceManager);
        }

        // wordt alleen ge-called vanuit het base object
        internal protected override ProjectReferenceNode GetGroupNode(BaseTfsXmlDocument tfsXmlDocument)
        {
            return new ProjectReferenceNode(tfsXmlDocument);
        }
    }
}
