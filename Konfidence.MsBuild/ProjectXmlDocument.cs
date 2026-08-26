using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using JetBrains.Annotations;
using Konfidence.Base;
using Konfidence.MsBuild.Project;
using Konfidence.MsBuild.DocumentBase;

namespace Konfidence.MsBuild
{
    public class ProjectXmlDocument : BaseProjectXmlDocument
    {
        private ContentItemNodeList _projectFileItemNodeList;
        private DllReferenceItemNodeList _dllReferenceItemNodeList;
        private ProjectReferenceItemNodeList _projectReferenceItemGroupList;
        private ProjectCompileItemNodeList _projectCompileItemNodeList;
        private ProjectNoneItemNodeList _projectNoneItemNodeList;
        private PropertyConfigurationItemNodeList _projectPropertyConfigurationNodeList;

        private static Dictionary<string, string> _projectGuidDictionary;

        private string _projectGuid = string.Empty;

        [NotNull]
        public string ProjectGuid
        {
            get
            {
                if (!_projectGuid.IsAssigned())
                {
                    _projectGuid = ProjectPropertyConfigurationNodeList.ProjectGuid;
                }
                return _projectGuid;
            }
            set
            {
                ProjectPropertyConfigurationNodeList.ProjectGuid = value;
            }
        }

        public string ProjectName => Path.GetFileNameWithoutExtension(FileName);

        #region properties

        [NotNull]
        public string GetRelativeProjectFileName(string basePath)
        {
            var projectPath = Path.GetDirectoryName(FileName);

            if (projectPath != null)
            {
                var relativePath = projectPath.Replace(basePath, string.Empty);

                if (!relativePath.EndsWith(@"\"))
                {
                    relativePath += @"\";
                }

                if (relativePath.StartsWith(@"\"))
                {
                    relativePath = relativePath.Substring(1);
                }

                return relativePath + Path.GetFileName(FileName);
            }

            return string.Empty;
        }

        [NotNull]
        internal PropertyConfigurationItemNodeList ProjectPropertyConfigurationNodeList
        {
            get
            {
                if (!_projectPropertyConfigurationNodeList.IsAssigned())
                {
                    _projectPropertyConfigurationNodeList = new PropertyConfigurationItemNodeList(this);
                }
                return _projectPropertyConfigurationNodeList;
            }
        }

        [NotNull]
        public ContentItemNodeList ProjectFileItemNodeList
        {
            get
            {
                if (!_projectFileItemNodeList.IsAssigned())
                {
                    _projectFileItemNodeList = new ContentItemNodeList(this);
                }
                return _projectFileItemNodeList;
            }
        }

        [NotNull]
        public DllReferenceItemNodeList DllReferenceItemNodeList
        {
            get
            {
                if (!_dllReferenceItemNodeList.IsAssigned())
                {
                    _dllReferenceItemNodeList = new DllReferenceItemNodeList(this);
                }
                return _dllReferenceItemNodeList;
            }
        }

        [NotNull]
        public ProjectReferenceItemNodeList ProjectReferenceItemNodeList
        {
            get
            {
                if (!_projectReferenceItemGroupList.IsAssigned())
                {
                    _projectReferenceItemGroupList = new ProjectReferenceItemNodeList(this);
                }
                return _projectReferenceItemGroupList;
            }
        }

        [NotNull]
        public ProjectCompileItemNodeList ProjectCompileItemNodeList
        {
            get
            {
                if (!_projectCompileItemNodeList.IsAssigned())
                {
                    _projectCompileItemNodeList = new ProjectCompileItemNodeList(this);
                }
                return _projectCompileItemNodeList;
            }
        }

        [NotNull]
        public ProjectNoneItemNodeList ProjectNoneItemNodeList
        {
            get
            {
                if (!_projectNoneItemNodeList.IsAssigned())
                {
                    _projectNoneItemNodeList = new ProjectNoneItemNodeList(this);
                }
                return _projectNoneItemNodeList;
            }
        }

        [NotNull]
        protected Dictionary<string, string> ProjectGuidDictionary
        {
            get
            {
                if (!_projectGuidDictionary.IsAssigned())
                {
                    _projectGuidDictionary = new Dictionary<string, string>();
                }

                return _projectGuidDictionary;
            }
        }

        #endregion properties

        private ProjectXmlDocument()
        {
        }

        [NotNull]
        public static ProjectXmlDocument GetProjectXmlDocument([NotNull] string projectFile)
        {
            var newProjectXmlDocument = new ProjectXmlDocument();

            newProjectXmlDocument.Load(projectFile);

            return newProjectXmlDocument;
        }

        public XmlElement AddDllReferenceElement(ReferenceItem referenceItem)
        {
            var referenceElement = DllReferenceItemNodeList.AppendChild();

            AddRequiredElements(referenceElement, referenceItem);

            return referenceElement;
        }

        [NotNull]
        public XmlElement AddProjectReferenceElement([NotNull] ReferenceItem referenceItem)
        {
            var projectGuid = GetProjectGuid(referenceItem.IncludeAttribute);

            var referenceElement = ProjectReferenceItemNodeList.AppendChild(projectGuid);

            AddRequiredElements(referenceElement, referenceItem);

            return referenceElement;
        }

        [CanBeNull]
        public XmlElement AddProjectFileElement([NotNull] ProjectFileItem projectFileItem)
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


        private string GetProjectGuid([NotNull] string projectFile)
        {
            string projectGuid;

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
                    var projectXmlDocument = new ProjectXmlDocument();

                    projectXmlDocument.Load(projectFile);

                    projectGuid = projectXmlDocument.GetProjectGuid();
                }

                ProjectGuidDictionary.Add(projectFile, projectGuid);
            }

            return projectGuid;
        }

        [NotNull]
        private string GetProjectGuid()
        {
            var propertyGroupList = Root.SelectNodes("p:PropertyGroup", XmlNamespaceManager);

            if (propertyGroupList.IsAssigned())
            {
                foreach (XmlNode propertyGroup in propertyGroupList)
                {
                    var projectGuidNode = propertyGroup.SelectSingleNode("p:ProjectGuid", XmlNamespaceManager);

                    if (projectGuidNode.IsAssigned())
                    {
                        return projectGuidNode.InnerText;
                    }
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

        private void AddNameElement(XmlElement referenceElement, [NotNull] ReferenceItem referenceItem)
        {
            if (referenceItem.Name.IsAssigned())
            {
                SetValue(referenceElement, "Name", referenceItem.Name);
            }
        }

        private void AddIncludeAttribute(XmlElement referenceElement, [NotNull] ReferenceItem referenceItem)
        {
            if (!string.IsNullOrEmpty(referenceItem.IncludeAttribute))
            {
                var includeAttribute = CreateAttribute("Include");

                referenceElement.Attributes.Append(includeAttribute);

                includeAttribute.InnerText = referenceItem.IncludeAttribute;
            }
        }

        private void AddHintPathElement(XmlElement referenceElement, [NotNull] ReferenceItem referenceItem)
        {
            if (referenceItem.HintPathElement.IsAssigned())
            {
                SetValue(referenceElement, "HintPath", referenceItem.HintPathElement);
            }
        }

        private void AddSpecificVersionElement(XmlElement referenceElement, [NotNull] ReferenceItem referenceItem)
        {
            if (referenceItem.SpecificVersionElement.IsAssigned())
            {
                SetValue(referenceElement, "SpecificVersion", referenceItem.SpecificVersionElement);
            }
        }

        [NotNull]
        public List<string> GetProjectFileNameList()
        {
            var fileList = new List<string>();

            foreach (var content in ProjectFileItemNodeList)
            {
                if (content.Include.IsAssigned())
                {
                    fileList.Add(content.Include);
                }
            }

            return fileList;
        }

        [NotNull]
        public List<string> GetProjectFileNameList([NotNull] List<string> endsWithFilter)
        {
            var fileList = new List<string>();

            foreach (var filter in endsWithFilter)
            {
                foreach (var content in ProjectFileItemNodeList)
                {
                    if (content.Include.IsAssigned())
                    {
                        if (content.Include.EndsWith(filter, StringComparison.InvariantCultureIgnoreCase))
                        {
                            fileList.Add(content.Include);
                        }
                    }
                }
            }

            return fileList;
        }
    }
}
