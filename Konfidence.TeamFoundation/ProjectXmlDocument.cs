﻿using System.Xml;
using System.Collections.Generic;
using Konfidence.TeamFoundation.Project;
using Konfidence.TeamFoundation.ProjectBase;

namespace Konfidence.TeamFoundation
{
    public class ProjectXmlDocument : BaseTfsXmlDocument
    {
        private const string PROJECT_REFERENCE_ITEMGROUP_NAME = "ProjectReference";
        private const string PROJECT_COMPILE_ITEMGROUP_NAME = "Compile";

        private DllReferenceItemNodeList _DllReferenceItemNodeList = null;
        private ProjectReferenceItemNodeList _ProjectReferenceItemGroupList = null;

        public ProjectXmlDocument()
        {
        }

        public DllReferenceItemNodeList DllReferenceItemNodeList
        {
            get
            {
                if (!IsAssigned(_DllReferenceItemNodeList))
                {
                    _DllReferenceItemNodeList = new DllReferenceItemNodeList(this);
                }
                return _DllReferenceItemNodeList;
            }
        }

        public XmlElement AddReferenceElement()
        {
            return DllReferenceItemNodeList.AppendChild();
        }

        // TODO : when a ProjectReferenceNode is added to the Xml it must also be added to the list
        // TODO : XmlNodeList omzetten naar List<ProjectReferenceNode>
        public ProjectReferenceItemNodeList ProjectReferenceItemNodeList
        {
            get
            {
                if (!IsAssigned(_ProjectReferenceItemGroupList))
                {
                    _ProjectReferenceItemGroupList = new ProjectReferenceItemNodeList(this);
                }
                return _ProjectReferenceItemGroupList;
            }
        }

        // TODO : when a CompileProjectNode is added to the Xml it must also be added to the list
        // TODO : XmlNodeList omzetten naar List<CompileProjectNode>
        public List<XmlNode> CompileProjectItemGroupList
        {
            get
            {
                return GetItemGroupList(PROJECT_COMPILE_ITEMGROUP_NAME);
            }
        }
    }
}
