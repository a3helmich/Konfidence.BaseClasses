using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;
using System.Xml;

namespace Konfidence.TeamFoundation
{
    public class BaseItemNode : BaseItem
    {
        private XmlNode _TfsXmlNode = null;

        public XmlNode TfsXmlNode
        {
            get { return _TfsXmlNode; }
        }

        public BaseItemNode(XmlNode xmlNode)
        {
            _TfsXmlNode = xmlNode;
        }
    }
}
