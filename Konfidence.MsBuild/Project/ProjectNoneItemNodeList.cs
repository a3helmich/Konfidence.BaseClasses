using System.Xml;
using JetBrains.Annotations;
using Konfidence.MsBuild.ProjectBase;
using Konfidence.MsBuild.DocumentBase;

namespace Konfidence.MsBuild.Project
{
    public class ProjectNoneItemNodeList : ProjectItemNodeList<ProjectNoneItemNode, ProjectNoneNode>
    {
        private readonly BaseProjectXmlDocument _projectXmlDocument;

        public ProjectNoneItemNodeList(BaseProjectXmlDocument projectXmlDocument)
            : base(projectXmlDocument)
        {
            _projectXmlDocument = projectXmlDocument;
        }

        // wordt alleen ge-called vanuit het base object
        [NotNull]
        protected internal override ProjectNoneItemNode GetItemNode(XmlNode projectItemNode, XmlNamespaceManager xmlNamespaceManager)
        {
            return new ProjectNoneItemNode(projectItemNode);
        }

        // wordt alleen ge-called vanuit het base object
        [NotNull]
        protected internal override ProjectNoneNode GetGroupNode(BaseProjectXmlDocument projectXmlDocument)
        {
            return new ProjectNoneNode(projectXmlDocument);
        }

        // wordt alleen ge-called vanuit het base object
        [NotNull]
        protected internal override ProjectNoneNode CreateGroupNode([NotNull] BaseProjectXmlDocument projectXmlDocument)
        {
            XmlNode itemGroupNode = projectXmlDocument.CreateElement("ItemGroup", projectXmlDocument.Root.NamespaceURI);

            return new ProjectNoneNode(projectXmlDocument, itemGroupNode);
        }

        [NotNull]
        internal XmlElement AppendChild([NotNull] ProjectFileItem projectFileItem)
        {
            var compileElement = AppendChild();

            var includeAttribute = _projectXmlDocument.CreateAttribute("Include");

            includeAttribute.InnerText = projectFileItem.FileName;

            compileElement.Attributes.Append(includeAttribute);

            return compileElement;
        }
    }
}
