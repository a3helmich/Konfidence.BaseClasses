using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.TeamFoundation.ProjectBase;
using Konfidence.Base;

namespace Konfidence.TeamFoundation.Project
{
    public class DllReferenceGroupNode : ProjectGroupNode
    {
        private const string DLL_REFERENCE_ITEMGROUP_NAME = "Reference";

        public DllReferenceGroupNode(BaseTfsXmlDocument tfsXmlDocument)
            : base(DLL_REFERENCE_ITEMGROUP_NAME, tfsXmlDocument)
        {
        }
    }
}
