using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.TeamFoundation.ProjectBase;
using System.Xml;

namespace Konfidence.TeamFoundation.Project
{
    public class ContentItemNodeList : ProjectItemNodeList<ContentItemNode, ContentNode>
    {
        public ContentItemNodeList(BaseTfsXmlDocument tfsXmlDocument) : base(tfsXmlDocument)
        {
        }

        // wordt alleen ge-called vanuit het base object
        internal protected override ContentItemNode GetItemNode(XmlNode projectItemNode, XmlNamespaceManager xmlNamespaceManager)
        {
            return new ContentItemNode(projectItemNode, xmlNamespaceManager);
        }

        // wordt alleen ge-called vanuit het base object
        internal protected override ContentNode GetGroupNode(BaseTfsXmlDocument tfsXmlDocument)
        {
            return new ContentNode(tfsXmlDocument);
        }

        // wordt alleen ge-called vanuit het base object
        internal protected override ContentNode CreateGroupNode(BaseTfsXmlDocument tfsXmlDocument)
        {
            XmlNode itemGroupNode = tfsXmlDocument.CreateElement("ItemGroup", tfsXmlDocument.NameSpaceURI);

            return new ContentNode(tfsXmlDocument, itemGroupNode);
        }
    }
}
