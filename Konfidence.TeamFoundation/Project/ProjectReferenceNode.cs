using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.TeamFoundation.ProjectBase;

namespace Konfidence.TeamFoundation.Project
{
    class ProjectReferenceNode : ProjectNode
    {
        private const string PROJECT_REFERENCE_ITEMGROUP_NAME = "ProjectReference";

        public ProjectReferenceNode(BaseTfsXmlDocument tfsXmlDocument)
            : base(PROJECT_REFERENCE_ITEMGROUP_NAME, tfsXmlDocument)
        {
        }
    }
}
