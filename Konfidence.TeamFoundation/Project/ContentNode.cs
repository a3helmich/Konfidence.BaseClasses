using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.TeamFoundation.ProjectBase;
using System.Xml;

namespace Konfidence.TeamFoundation.Project
{
    public class ContentNode : ProjectNode
    {
        private const string DLL_CONTENT_ITEMGROUP_NAME = "Content";

        
        public ContentNode(BaseTfsXmlDocument tfsXmlDocument)
            : base(DLL_CONTENT_ITEMGROUP_NAME, tfsXmlDocument)
        {
        }

        internal ContentNode(BaseTfsXmlDocument tfsXmlDocument, XmlNode itemGroupNode)
            : base(DLL_CONTENT_ITEMGROUP_NAME, tfsXmlDocument, itemGroupNode)
        {
        }

    }
}
