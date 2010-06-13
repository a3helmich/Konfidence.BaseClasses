using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Konfidence.TeamFoundation.ProjectBase;

namespace Konfidence.TeamFoundation.Project
{
    public class DllReferenceItemNodeList : ProjectItemNodeList<DllReferenceItemNode, DllReferenceGroupNode>
    {
        public DllReferenceItemNodeList(BaseTfsXmlDocument tfsXmlDocument)
            : base(tfsXmlDocument)
        {
        }

        protected override DllReferenceItemNode GetNewItemNode(XmlNode projectItemNode, XmlNamespaceManager xmlNamespaceManager)
        {
            return new DllReferenceItemNode(projectItemNode, xmlNamespaceManager);
        }

        protected override DllReferenceGroupNode GetGroupNode(BaseTfsXmlDocument tfsXmlDocument)
        {
            return new DllReferenceGroupNode(tfsXmlDocument);
        }
    }
}
