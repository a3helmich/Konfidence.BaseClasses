using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.TeamFoundation.ProjectBase;
using System.Xml;

namespace Konfidence.TeamFoundation.Project
{
    class PropertyConfigurationItemNode : ProjectItemNode
    {
        private XmlNamespaceManager _XmlNamespaceManager;

        public PropertyConfigurationItemNode(XmlNode xmlNode, XmlNamespaceManager xmlNamespaceManager)
            : base(xmlNode)
        {
            _XmlNamespaceManager = xmlNamespaceManager;
        }

        internal XmlNode GetElement(string p)
        {
            throw new NotImplementedException();
        }
    }
}
