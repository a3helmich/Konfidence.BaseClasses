using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Konfidence.TeamFoundation.ProjectBase;

namespace Konfidence.TeamFoundation.Project
{
    public class ProjectCompileItemNode : ProjectItemNode
    {
        private XmlNamespaceManager _XmlNamespaceManager;

        public ProjectCompileItemNode(XmlNode xmlNode, XmlNamespaceManager xmlNamespaceManager)
            : base(xmlNode)
        {
            _XmlNamespaceManager = xmlNamespaceManager;
        }

        public string FileName
        {
            get
            {
                return TfsXmlNode.Attributes["Include"].InnerText;
            }
        }
    }
}
