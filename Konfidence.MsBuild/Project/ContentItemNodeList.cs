using System.Xml;
using JetBrains.Annotations;
using Konfidence.MsBuild.ProjectBase;
using Konfidence.MsBuild.DocumentBase;

namespace Konfidence.MsBuild.Project
{
    public class ContentItemNodeList : ProjectItemNodeList<ContentItemNode, ContentNode>
    {
        public ContentItemNodeList(BaseProjectXmlDocument projectXmlDocument) : base(projectXmlDocument)
        {
        }

        // wordt alleen ge-called vanuit het base object
        [NotNull]
        protected internal override ContentItemNode GetItemNode(XmlNode projectItemNode, XmlNamespaceManager xmlNamespaceManager)
        {
            return new ContentItemNode(projectItemNode, xmlNamespaceManager);
        }

        // wordt alleen ge-called vanuit het base object
        [NotNull]
        protected internal override ContentNode GetGroupNode(BaseProjectXmlDocument projectXmlDocument)
        {
            return new ContentNode(projectXmlDocument);
        }

        // wordt alleen ge-called vanuit het base object
        [NotNull]
        protected internal override ContentNode CreateGroupNode([NotNull] BaseProjectXmlDocument projectXmlDocument)
        {
            XmlNode itemGroupNode = projectXmlDocument.CreateElement("ItemGroup", projectXmlDocument.Root.NamespaceURI);

            return new ContentNode(projectXmlDocument, itemGroupNode);
        }
    }
}
