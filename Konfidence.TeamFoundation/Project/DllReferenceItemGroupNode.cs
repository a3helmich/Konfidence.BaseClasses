using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.TeamFoundation.ProjectBase;

namespace Konfidence.TeamFoundation.Project
{
    public class DllReferenceItemGroupNode : BaseItemGroupNode<DllReferenceNode>
    {
        private const string DLL_REFERENCE_ITEMGROUP_NAME = "Reference";

        public DllReferenceItemGroupNode(BaseTfsXmlDocument tfsXmlDocument)
            : base(DLL_REFERENCE_ITEMGROUP_NAME, tfsXmlDocument)
        {
        }
    }
}
