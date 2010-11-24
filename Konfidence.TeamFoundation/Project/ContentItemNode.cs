using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.TeamFoundation.ProjectBase;
using System.Xml;

namespace Konfidence.TeamFoundation.Project
{
    public class ContentItemNode : ProjectItemNode
    {
        //private XmlElement _Include = null;
        private XmlNamespaceManager _XmlNamespaceManager;

        #region readOnlyProperties
        public string Include
        {
            get
            {
                if (IsAssigned(TfsXmlNode.Attributes["Include"]))
                {
                    return TfsXmlNode.Attributes["Include"].InnerText;
                }

                return string.Empty;
            }
        }
        #endregion readOnlyProperties

        public ContentItemNode(XmlNode xmlNode, XmlNamespaceManager xmlNamespaceManager) : base(xmlNode)
        {
            _XmlNamespaceManager = xmlNamespaceManager;
        }
    }
}
