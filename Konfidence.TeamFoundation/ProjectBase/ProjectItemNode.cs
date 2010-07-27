using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;
using System.Xml;

namespace Konfidence.TeamFoundation.ProjectBase
{
    // encapsulation van XmlNode
    public abstract class ProjectItemNode : BaseItem
    {
        private XmlNode _TfsXmlNode = null;

        public XmlNode TfsXmlNode
        {
            get { return _TfsXmlNode; }
        }

        public ProjectItemNode(XmlNode xmlNode)
        {
            _TfsXmlNode = xmlNode;
        }

        //public virtual ProjectItemNode GetNewItem(XmlNode xmlNode, XmlNamespaceManager xmlNamespaceManager)
        //{
        //    ProjectItemNode newItem = null;

        //    return newItem;
        //}
    }
}
