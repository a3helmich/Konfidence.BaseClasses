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
    public class ProjectFileProcessor: BaseItem
    {
        private ProjectXmlDocument _ProjectXmlDocument = null;
        private string _ProjectFileName = string.Empty;

        #region simple properties
        public string ProjectFileName
        {
            get { return _ProjectFileName; }
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
                    fileList.Add(projectFolder + @"\" + fileName);
                }
            }

            fileList.Sort();

            return fileList;
        }
    }
}
