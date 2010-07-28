using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.TeamFoundation.ProjectBase;
using System.Xml;

namespace Konfidence.TeamFoundation.Project
{
    public class DllReferenceNode : ProjectNode
    {
        private const string DLL_REFERENCE_ITEMGROUP_NAME = "Reference";

        public DllReferenceNode(BaseTfsXmlDocument tfsXmlDocument)
            : base(DLL_REFERENCE_ITEMGROUP_NAME, tfsXmlDocument)
        {
        }

        internal DllReferenceNode(BaseTfsXmlDocument tfsXmlDocument, XmlNode itemGroupNode)
            : base(DLL_REFERENCE_ITEMGROUP_NAME, tfsXmlDocument, itemGroupNode)
        {
        }
    }
}
