using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.TeamFoundation;
using Konfidence.TeamFoundation.Project;
using Konfidence.Base;
using System.IO;

namespace WebProjectValidator.HelperClasses
{
    public class ProjectFileProcessor : BaseItem
    {
        private ProjectXmlDocument _ProjectXmlDocument = null;
        private string _ProjectFileName = string.Empty;
        private bool _AllFolders = false;

        #region simple properties
        public string ProjectFileName
        {
            get { return _ProjectFileName; }
        }

        public bool AllFolders
        {
            get { return _AllFolders; }
            set { _AllFolders = value; }
        }
        #endregion

        public ProjectFileProcessor(string projectFile)
        {
            if (File.Exists(projectFile))
            {
                _ProjectXmlDocument = new ProjectXmlDocument();

                _ProjectXmlDocument.Load(projectFile);
            }
        }

        public List<string> GetProjectFileNameList(string projectFolder, List<string> extensionFilter)
        {
            List<string> fileList = new List<string>();

            if (IsAssigned(_ProjectXmlDocument))
            {
                foreach (string fileName in _ProjectXmlDocument.GetProjectFileNameList(extensionFilter))
                {
                    if (InAllowedFolder(fileName))
                    {
                        fileList.Add(projectFolder + @"\" + fileName);
                    }
                }
            }

            fileList.Sort();

            return fileList;
        }

        private bool InAllowedFolder(string fileName)
        {
            if (_AllFolders)
            {
                return true;
            }

            string testName = fileName.ToLower();

            if (testName.StartsWith("app_data"))
            {
                return false;
            }

            if (testName.StartsWith("theme"))
            {
                return false;
            }

            if (testName.StartsWith("app_browsers"))
            {
                return false;
            }

            if (testName.StartsWith("app_globalresources"))
            {
                return false;
            }

            if (testName.StartsWith("app_localresources"))
            {
                return false;
            }

            return true;
        }
    }
}
