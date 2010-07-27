﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Konfidence.TeamFoundation.ProjectBase;

namespace Konfidence.TeamFoundation.Project
{
    public class DllReferenceItemNodeList : ProjectItemNodeList<DllReferenceItemNode, DllReferenceNode>
    {
        public DllReferenceItemNodeList(BaseTfsXmlDocument tfsXmlDocument)
            : base(tfsXmlDocument)
        {
        }

        // wordt alleen ge-called vanuit het base object
        protected override DllReferenceItemNode GetItemNode(XmlNode projectItemNode, XmlNamespaceManager xmlNamespaceManager)
        {
            return new DllReferenceItemNode(projectItemNode, xmlNamespaceManager);
        }

        // wordt alleen ge-called vanuit het base object
        protected override DllReferenceNode GetGroupNode(BaseTfsXmlDocument tfsXmlDocument)
        {
            return new DllReferenceNode(tfsXmlDocument);
        }
    }
}