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
        private BaseTfsXmlDocument _TfsXmlDocument = null;

        public ProjectCompileItemNodeList(BaseTfsXmlDocument tfsXmlDocument)
            : base(tfsXmlDocument)
        {
            _TfsXmlDocument = tfsXmlDocument;
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

        internal XmlElement AppendChild(ProjectFileItem projectFileItem)
        {
            XmlElement compileElement = base.AppendChild();
            
            //compileElement.

            XmlAttribute includeAttribute = _TfsXmlDocument.CreateAttribute("Include");

            includeAttribute.InnerText = projectFileItem.FileName;

            compileElement.Attributes.Append(includeAttribute);

            return compileElement;
        }
    }
}
