using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Konfidence.TeamFoundation.ProjectBase;

namespace Konfidence.TeamFoundation.Project
{
    public class ProjectCompileItemNodeList : ProjectItemNodeList<ProjectCompileItemNode, ProjectCompileNode>
    {
        public ProjectCompileItemNodeList(BaseTfsXmlDocument tfsXmlDocument)
            : base(tfsXmlDocument)
        {
        }

        // wordt alleen ge-called vanuit het base object
        internal protected override ProjectCompileItemNode GetItemNode(XmlNode projectItemNode, XmlNamespaceManager xmlNamespaceManager)
        {
            return new ProjectCompileItemNode(projectItemNode, xmlNamespaceManager);
        }

        // wordt alleen ge-called vanuit het base object
        internal protected override ProjectCompileNode GetGroupNode(BaseTfsXmlDocument tfsXmlDocument)
        {
            return new ProjectCompileNode(tfsXmlDocument);
        }

        // wordt alleen ge-called vanuit het base object
        internal protected override ProjectCompileNode CreateGroupNode(BaseTfsXmlDocument tfsXmlDocument)
        {
            XmlNode itemGroupNode = tfsXmlDocument.CreateElement("ItemGroup", tfsXmlDocument.NameSpaceURI);

            return new ProjectCompileNode(tfsXmlDocument, itemGroupNode);
        }
    }
}
