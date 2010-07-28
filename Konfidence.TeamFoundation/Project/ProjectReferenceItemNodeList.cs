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
        private BaseTfsXmlDocument _TfsXmlDocument = null;

        public ProjectReferenceItemNodeList(BaseTfsXmlDocument tfsXmlDocument)
            : base(tfsXmlDocument)
        {
            _TfsXmlDocument = tfsXmlDocument;
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

        // wordt alleen ge-called vanuit het base object
        internal protected override ProjectReferenceNode CreateGroupNode(BaseTfsXmlDocument tfsXmlDocument)
        {
            XmlNode itemGroupNode = tfsXmlDocument.CreateElement("ItemGroup", tfsXmlDocument.NameSpaceURI);

            return new ProjectReferenceNode(tfsXmlDocument, itemGroupNode);
        }

        internal protected XmlElement AppendChild(XmlElement projectElement)
        {
            XmlElement referenceElement = base.AppendChild();

            referenceElement.AppendChild(projectElement);

            return referenceElement;
        }
    }
}
