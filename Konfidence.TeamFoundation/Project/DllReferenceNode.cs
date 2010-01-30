using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Konfidence.TeamFoundation.Project
{
    internal class DllReferenceNode : BaseTfsXmlNode
    {
        private XmlElement _HintPath = null;

        public XmlElement HintPath
        {
            get 
            {
                if (!IsAssigned(_HintPath))
                {
                    XmlElement hintPath = TfsXmlNode.SelectSingleNode("p:HintPath", XmlNamespaceManager) as XmlElement;
                }

                return _HintPath; 
            }
        }

        public DllReferenceNode(XmlElement xmlElement): base(xmlElement)
        {
        }
    }
}
