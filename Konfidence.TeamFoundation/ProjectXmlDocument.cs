using System;
using System.Collections.Generic;
using System.Xml;

using Konfidence.TeamFoundation.Project;
using Konfidence.TeamFoundation.ProjectBase;

namespace Konfidence.TeamFoundation
{
    public class ProjectXmlDocument : BaseTfsXmlDocument
    {
        private DllReferenceItemNodeList _DllReferenceItemNodeList = null;
        private ProjectReferenceItemNodeList _ProjectReferenceItemGroupList = null;
        private CompileProjectItemNodeList _CompileProjectItemNodeList = null;

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

        public CompileProjectItemNodeList CompileProjectItemNodeList
        {
            get
            {
                if (!IsAssigned(_CompileProjectItemNodeList))
                {
                    _CompileProjectItemNodeList = new CompileProjectItemNodeList(this);
                }
                return _CompileProjectItemNodeList;
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

        public XmlElement AddDllReferenceElement(ReferenceItem referenceItem)
        {
            XmlElement referenceElement = DllReferenceItemNodeList.AppendChild();

            AddRequiredElements(referenceElement, referenceItem);

            return referenceElement;
        }

        public XmlElement AddProjectReferenceElement(ReferenceItem referenceItem)
        {
            XmlElement projectElement = CreateElement("Project", NameSpaceURI);

            projectElement.InnerText = GetProjectGuid(referenceItem.IncludeAttribute);

            XmlElement referenceElement = ProjectReferenceItemNodeList.AppendChild(projectElement);

            referenceElement.AppendChild(projectElement);

            AddRequiredElements(referenceElement, referenceItem);

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

        private void AddRequiredElements(XmlElement referenceElement, ReferenceItem referenceItem)
        {
            AddSpecificVersionElement(referenceElement, referenceItem);

            AddHintPathElement(referenceElement, referenceItem);

            AddIncludeAttribute(referenceElement, referenceItem);

            AddNameElement(referenceElement, referenceItem);
        }

        private void AddNameElement(XmlElement referenceElement, ReferenceItem referenceItem)
        {
            if (IsAssigned(referenceItem.Name))
            {
                XmlElement nameElement = CreateElement("Name", NameSpaceURI);

                referenceElement.AppendChild(nameElement);

                nameElement.InnerText = referenceItem.Name;
            }
        }

        private void AddIncludeAttribute(XmlElement referenceElement, ReferenceItem referenceItem)
        {
            if (!string.IsNullOrEmpty(referenceItem.IncludeAttribute))
            {
                XmlAttribute includeAttribute = CreateAttribute("Include");

                referenceElement.Attributes.Append(includeAttribute);

                includeAttribute.InnerText = referenceItem.IncludeAttribute;
            }
        }

        private void AddHintPathElement(XmlElement referenceElement, ReferenceItem referenceItem)
        {
            if (!string.IsNullOrEmpty(referenceItem.HintPathElement))
            {

                XmlElement hintPathElement = CreateElement("HintPath", NameSpaceURI);

                referenceElement.AppendChild(hintPathElement);

                hintPathElement.InnerText = referenceItem.HintPathElement;
            }
        }

        private void AddSpecificVersionElement(XmlElement referenceElement, ReferenceItem referenceItem)
        {
            if (!string.IsNullOrEmpty(referenceItem.SpecificVersionElement))
            {
                XmlElement specificVersionElement = CreateElement("SpecificVersion", NameSpaceURI);

                referenceElement.AppendChild(specificVersionElement);

                specificVersionElement.InnerText = referenceItem.SpecificVersionElement;
            }
        }
    }
}
