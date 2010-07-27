using System;
using System.Collections.Generic;
using System.Xml;

using Konfidence.TeamFoundation.Project;
using Konfidence.TeamFoundation.ProjectBase;

namespace Konfidence.TeamFoundation
{
    public class ProjectXmlDocument : BaseTfsXmlDocument
    {
        private const string PROJECT_COMPILE_ITEMGROUP_NAME = "Compile";

        private DllReferenceItemNodeList _DllReferenceItemNodeList = null;
        private ProjectReferenceItemNodeList _ProjectReferenceItemGroupList = null;

        private static Dictionary<string, string> _ProjectGuidDictionary = null;

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

        protected Dictionary<string, string> ProjectGuidDictionary
        {
            get
            {
                if (!IsAssigned(_ProjectGuidDictionary))
                {
                    _ProjectGuidDictionary = new Dictionary<string, string>();
                }

                return _ProjectGuidDictionary;
            }
        }

        public ProjectXmlDocument()
        {
        }

        public XmlElement AddReferenceElement()
        {
            return DllReferenceItemNodeList.AppendChild();
        }

        public XmlElement AddProjectReferenceElement(ProjectAssemblyItem assemblyItem)
        {
            XmlElement projectElement = CreateElement("Project", NameSpaceURI);

            projectElement.InnerText = GetProjectGuid(assemblyItem.IncludeAttribute);

            XmlElement referenceElement = ProjectReferenceItemNodeList.AppendChild(projectElement);

            referenceElement.AppendChild(projectElement);

            return referenceElement;
        }

        private string GetProjectGuid(string projectFile)
        {
            string projectGuid = string.Empty;

            if (ProjectGuidDictionary.ContainsKey(projectFile))
            {
                projectGuid = ProjectGuidDictionary[projectFile];
            }
            else
            {
                if (projectFile.Equals(FileName))
                {
                    projectGuid = GetProjectGuid();
                }
                else
                {
                    ProjectXmlDocument projectXmlDocument = new ProjectXmlDocument();

                    projectXmlDocument.Load(projectFile);

                    projectGuid = projectXmlDocument.GetProjectGuid();
                }

                ProjectGuidDictionary.Add(projectFile, projectGuid);
            }

            return projectGuid;
        }

        private string GetProjectGuid()
        {
            XmlNodeList propertyGroupList = Root.SelectNodes("p:PropertyGroup", XmlNamespaceManager);

            foreach (XmlNode propertyGroup in propertyGroupList)
            {
                XmlNode projectGuidNode = propertyGroup.SelectSingleNode("p:ProjectGuid", XmlNamespaceManager);

                if (IsAssigned(projectGuidNode))
                {
                    return projectGuidNode.InnerText;
                }
            }

            return Guid.Empty.ToString("B");
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
