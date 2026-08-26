using System.Xml;
using JetBrains.Annotations;
using Konfidence.MsBuild.ProjectBase;
using Konfidence.MsBuild.DocumentBase;

namespace Konfidence.MsBuild.Project
{
    public class ProjectCompileItemNodeList : ProjectItemNodeList<ProjectCompileItemNode, ProjectCompileNode>
    {
        private readonly BaseProjectXmlDocument _projectXmlDocument;

        public ProjectCompileItemNodeList(BaseProjectXmlDocument projectXmlDocument): base(projectXmlDocument)
        {
            _projectXmlDocument = projectXmlDocument;
        }

        // wordt alleen ge-called vanuit het base object
        [NotNull]
        protected internal override ProjectCompileItemNode GetItemNode(XmlNode projectItemNode, XmlNamespaceManager xmlNamespaceManager)
        {
            return new ProjectCompileItemNode(projectItemNode, xmlNamespaceManager);
        }

        // wordt alleen ge-called vanuit het base object
        [NotNull]
        protected internal override ProjectCompileNode GetGroupNode(BaseProjectXmlDocument projectXmlDocument)
        {
            return new ProjectCompileNode(projectXmlDocument);
        }

        // wordt alleen ge-called vanuit het base object
        [NotNull]
        protected internal override ProjectCompileNode CreateGroupNode([NotNull] BaseProjectXmlDocument projectXmlDocument)
        {
            XmlNode itemGroupNode = projectXmlDocument.CreateElement("ItemGroup", projectXmlDocument.Root.NamespaceURI);

            return new ProjectCompileNode(projectXmlDocument, itemGroupNode);
        }

        [NotNull]
        internal XmlElement AppendChild([NotNull] ProjectFileItem projectFileItem)
        {
            var compileElement = AppendChild();
            
            // ToDo de naam van het compileElement omzetten naar de meegegeven naam in het projectfileitem
            //compileElement.
            //compileElement.Name = projectFileItem.Action;

            var includeAttribute = _projectXmlDocument.CreateAttribute("Include");

            includeAttribute.InnerText = projectFileItem.FileName;

            compileElement.Attributes.Append(includeAttribute);

            return compileElement;
        }
    }
}
