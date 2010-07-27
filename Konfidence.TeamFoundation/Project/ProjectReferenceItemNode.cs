using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Konfidence.TeamFoundation.ProjectBase;

namespace Konfidence.TeamFoundation.Project
{
    class ProjectReferenceItemNode : ProjectItemNode
    {
        private XmlNamespaceManager _XmlNamespaceManager;

        public ProjectReferenceItemNode(XmlNode xmlNode, XmlNamespaceManager xmlNamespaceManager)
            : base(xmlNode)
        {
            _XmlNamespaceManager = xmlNamespaceManager;
        }
    }
}
