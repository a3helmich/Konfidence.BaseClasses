using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Konfidence.TeamFoundation.ProjectBase;

namespace Konfidence.TeamFoundation.Project
{
    public class ProjectNoneItemNodeList : ProjectItemNodeList<ProjectNoneItemNode, ProjectNoneNode>
    {
        private BaseTfsXmlDocument _TfsXmlDocument = null;

        public ProjectNoneItemNodeList(BaseTfsXmlDocument tfsXmlDocument)
            : base(tfsXmlDocument)
        {
            _TfsXmlDocument = tfsXmlDocument;
        }

        // wordt alleen ge-called vanuit het base object
        internal protected override ProjectNoneItemNode GetItemNode(XmlNode projectItemNode, XmlNamespaceManager xmlNamespaceManager)
        {
            return new ProjectNoneItemNode(projectItemNode, xmlNamespaceManager);
        }

        // wordt alleen ge-called vanuit het base object
        internal protected override ProjectNoneNode GetGroupNode(BaseTfsXmlDocument tfsXmlDocument)
        {
            return new ProjectNoneNode(tfsXmlDocument);
        }

        // wordt alleen ge-called vanuit het base object
        internal protected override ProjectNoneNode CreateGroupNode(BaseTfsXmlDocument tfsXmlDocument)
        {
            XmlNode itemGroupNode = tfsXmlDocument.CreateElement("ItemGroup", tfsXmlDocument.NameSpaceURI);

            return new ProjectNoneNode(tfsXmlDocument, itemGroupNode);
        }

        internal XmlElement AppendChild(ProjectFileItem projectFileItem)
        {
            XmlElement compileElement = base.AppendChild();

            XmlAttribute includeAttribute = _TfsXmlDocument.CreateAttribute("Include");

            includeAttribute.InnerText = projectFileItem.FileName;

            compileElement.Attributes.Append(includeAttribute);

            return compileElement;
        }
    }
}
