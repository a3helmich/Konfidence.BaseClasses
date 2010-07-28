using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Konfidence.TeamFoundation.ProjectBase;

namespace Konfidence.TeamFoundation.Project
{
    public class CompileProjectItemNodeList : ProjectItemNodeList<CompileProjectItemNode, CompileProjectNode>
    {
        public CompileProjectItemNodeList(BaseTfsXmlDocument tfsXmlDocument)
            : base(tfsXmlDocument)
        {
        }

        // wordt alleen ge-called vanuit het base object
        internal protected override CompileProjectItemNode GetItemNode(XmlNode projectItemNode, XmlNamespaceManager xmlNamespaceManager)
        {
            return new CompileProjectItemNode(projectItemNode, xmlNamespaceManager);
        }

        // wordt alleen ge-called vanuit het base object
        internal protected override CompileProjectNode GetGroupNode(BaseTfsXmlDocument tfsXmlDocument)
        {
            return new CompileProjectNode(tfsXmlDocument);
        }

        // wordt alleen ge-called vanuit het base object
        internal protected override CompileProjectNode CreateGroupNode(BaseTfsXmlDocument tfsXmlDocument)
        {
            XmlNode itemGroupNode = tfsXmlDocument.CreateElement("ItemGroup", tfsXmlDocument.NameSpaceURI);

            return new CompileProjectNode(tfsXmlDocument, itemGroupNode);
        }
    }
}
