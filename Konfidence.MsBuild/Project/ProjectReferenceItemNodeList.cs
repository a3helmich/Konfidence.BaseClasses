using System.Xml;
using JetBrains.Annotations;
using Konfidence.MsBuild.ProjectBase;
using Konfidence.MsBuild.DocumentBase;

namespace Konfidence.MsBuild.Project
{
    public class ProjectReferenceItemNodeList : ProjectItemNodeList<ProjectReferenceItemNode, ProjectReferenceNode>
    {
        private readonly BaseProjectXmlDocument _projectXmlDocument;

        public ProjectReferenceItemNodeList(BaseProjectXmlDocument projectXmlDocument)
            : base(projectXmlDocument)
        {
            _projectXmlDocument = projectXmlDocument;
        }

        // wordt alleen ge-called vanuit het base object
        [NotNull]
        protected internal override ProjectReferenceItemNode GetItemNode(XmlNode projectItemNode, XmlNamespaceManager xmlNamespaceManager)
        {
            return new ProjectReferenceItemNode(projectItemNode);
        }

        // wordt alleen ge-called vanuit het base object
        [NotNull]
        protected internal override ProjectReferenceNode GetGroupNode(BaseProjectXmlDocument projectXmlDocument)
        {
            return new ProjectReferenceNode(projectXmlDocument);
        }

        // wordt alleen ge-called vanuit het base object
        [NotNull]
        protected internal override ProjectReferenceNode CreateGroupNode([NotNull] BaseProjectXmlDocument projectXmlDocument)
        {
            XmlNode itemGroupNode = projectXmlDocument.CreateElement("ItemGroup", projectXmlDocument.Root.NamespaceURI);

            return new ProjectReferenceNode(projectXmlDocument, itemGroupNode);
        }

        [NotNull]
        protected internal XmlElement AppendChild([NotNull] string projectGuid)
        {
            var referenceElement = AppendChild();

            var projectElement = _projectXmlDocument.CreateElement("Project", _projectXmlDocument.Root.NamespaceURI);

            projectElement.InnerText = projectGuid;

            referenceElement.AppendChild(projectElement);

            return referenceElement;
        }
    }
}
