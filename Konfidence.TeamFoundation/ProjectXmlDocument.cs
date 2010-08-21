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
        private ProjectCompileItemNodeList _ProjectCompileItemNodeList = null;
        private ProjectNoneItemNodeList _ProjectNoneItemNodeList = null;

        private static Dictionary<string, string> _ProjectGuidDictionary = null;

        #region properties
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

        public ProjectCompileItemNodeList ProjectCompileItemNodeList
        {
            get
            {
                if (!IsAssigned(_ProjectCompileItemNodeList))
                {
                    _ProjectCompileItemNodeList = new ProjectCompileItemNodeList(this);
                }
                return _ProjectCompileItemNodeList;
            }
        }

        public ProjectNoneItemNodeList ProjectNoneItemNodeList
        {
            get
            {
                if (!IsAssigned(_ProjectNoneItemNodeList))
                {
                    _ProjectNoneItemNodeList = new ProjectNoneItemNodeList(this);
                }
                return _ProjectNoneItemNodeList;
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

        #endregion properties

        public ProjectXmlDocument()
        {
        }

        public static ProjectXmlDocument GetProjectXmlDocument(string projectFile)
        {
            ProjectXmlDocument newProjectXmlDocument = new ProjectXmlDocument();

            newProjectXmlDocument.Load(projectFile);

            return newProjectXmlDocument;
        }

        public XmlElement AddDllReferenceElement(ReferenceItem referenceItem)
        {
            XmlElement referenceElement = DllReferenceItemNodeList.AppendChild();

            AddRequiredElements(referenceElement, referenceItem);

            return referenceElement;
        }

        public XmlElement AddProjectReferenceElement(ReferenceItem referenceItem)
        {
            string projectGuid = GetProjectGuid(referenceItem.IncludeAttribute);

            XmlElement referenceElement = ProjectReferenceItemNodeList.AppendChild(projectGuid);

            AddRequiredElements(referenceElement, referenceItem);

            return referenceElement;
        }

        public XmlElement AddProjectFileElement(ProjectFileItem projectFileItem)
        {
            XmlElement fileElement = null;

            switch (projectFileItem.Action.ToLower())
            {
                case "compile":
                    fileElement = ProjectCompileItemNodeList.AppendChild(projectFileItem);
                    break;
                case "none":
                    fileElement = ProjectNoneItemNodeList.AppendChild(projectFileItem);
                    break;
            }

            return fileElement;
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
                AddElement(referenceElement, "Name", referenceItem.Name);
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
            if (IsAssigned(referenceItem.HintPathElement))
            {
                AddElement(referenceElement, "HintPath", referenceItem.HintPathElement);
            }
        }

        private void AddSpecificVersionElement(XmlElement referenceElement, ReferenceItem referenceItem)
        {
            if (IsAssigned(referenceItem.SpecificVersionElement))
            {
                AddElement(referenceElement, "SpecificVersion", referenceItem.SpecificVersionElement);
            }
        }
    }
}
