using System.Xml;
using JetBrains.Annotations;
using Konfidence.MsBuild.ProjectBase;
using Konfidence.MsBuild.DocumentBase;

namespace Konfidence.MsBuild.Project
{
    public class DllReferenceItemNodeList : ProjectItemNodeList<DllReferenceItemNode, DllReferenceNode>
    {
        public DllReferenceItemNodeList(BaseProjectXmlDocument projectXmlDocument) : base(projectXmlDocument)
        {
        }

        // wordt alleen ge-called vanuit het base object
        [NotNull]
        protected internal override DllReferenceItemNode GetItemNode(XmlNode projectItemNode, XmlNamespaceManager xmlNamespaceManager)
        {
            return new DllReferenceItemNode(projectItemNode, xmlNamespaceManager);
        }

        // wordt alleen ge-called vanuit het base object
        [NotNull]
        protected internal override DllReferenceNode GetGroupNode(BaseProjectXmlDocument projectXmlDocument)
        {
            return new DllReferenceNode(projectXmlDocument);
        }

        // wordt alleen ge-called vanuit het base object
        [NotNull]
        protected internal override DllReferenceNode CreateGroupNode([NotNull] BaseProjectXmlDocument projectXmlDocument)
        {
            XmlNode itemGroupNode = projectXmlDocument.CreateElement("ItemGroup", projectXmlDocument.Root.NamespaceURI);

            return new DllReferenceNode(projectXmlDocument, itemGroupNode);
        }
    }
}
