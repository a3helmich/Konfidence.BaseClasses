using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.TeamFoundation;
using Konfidence.TeamFoundation.Project;
using Konfidence.Base;

namespace WebProjectValidator.HelperClasses
{
    public class ProjectFileProcessor: BaseItem
    {
        ProjectXmlDocument projectXmlDocument = new ProjectXmlDocument();

        public ProjectFileProcessor(string projectName, string folderName, LanguageType languageType)
        {
            string fileName;

            fileName = string.Empty;

            switch (languageType)
            {
                case LanguageType.cs:
                    fileName = folderName + @"\" + projectName + ".csproj";
                    break;
                case LanguageType.vb:
                    fileName = folderName + @"\" + projectName + ".vbproj";
                    break;
            }

            projectXmlDocument.Load(fileName);
        }

        public List<string> GetProjectFiles()
        {
            List<string> fileList = new List<string>();

            foreach (ContentItemNode content in projectXmlDocument.ProjectFileItemNodeList)
            {
                if (IsAssigned(content.Include))
                {
                    fileList.Add(content.Include);
                }
            }

            return fileList;
        }
    }
}
