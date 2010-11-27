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

        public ProjectFileProcessor(string projectName, string folderName, LanguageType languageType)
        {
            _ProjectFileName = string.Empty;

            switch (languageType)
            {
                case LanguageType.cs:
                    _ProjectFileName = folderName + @"\" + projectName + ".csproj";
                    break;
                case LanguageType.vb:
                    _ProjectFileName = folderName + @"\" + projectName + ".vbproj";
                    break;
            }

            if (File.Exists(_ProjectFileName))
            {
                _ProjectXmlDocument = new ProjectXmlDocument();

                _ProjectXmlDocument.Load(_ProjectFileName);
            }
            else
            {
                _ProjectFileName = "Projectfile niet gevonden: " + _ProjectFileName;
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

            return fileList;
        }
    }
}
