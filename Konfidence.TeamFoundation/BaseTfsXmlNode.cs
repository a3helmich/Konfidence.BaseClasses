using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;
using System.Xml;

namespace Konfidence.TeamFoundation
{
    public class BaseTfsXmlNode : BaseItem
    {
        private XmlNode _TfsXmlNode = null;

        public XmlNode TfsXmlNode
        {
            get { return _TfsXmlNode; }
        }

        public BaseTfsXmlNode(XmlNode xmlNode)
        {
            _TfsXmlNode = xmlNode;
        }
    }
}
