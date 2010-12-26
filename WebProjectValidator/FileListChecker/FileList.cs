using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WebProjectValidator.EnumTypes;

namespace WebProjectValidator.FileListChecker
{
    public class FileList : List<string>
    {
        private FolderFilterList _FolderFilterList = new FolderFilterList();

        public FileList(string projectFolder, LanguageType languageType, DeveloperFileType developerType)
        {
            CheckItemList checker = GetChecker(languageType, developerType);

            foreach (string fileName in GetFileList(projectFolder))
            {
                if (checker.IsValid(fileName) && !_FolderFilterList.IsFiltered(fileName))
                {
                    this.Add(fileName);
                }
            }
        }

        private List<string> GetFileList(string projectFolder)
        {
            List<string> tempList = new List<string>();

            tempList.AddRange(Directory.GetFiles(projectFolder, "*.*", SearchOption.AllDirectories));

            return tempList;
        }

        private CheckItemList GetChecker(LanguageType languageType, DeveloperFileType developerType)
        {
            switch (developerType)
            {
                case DeveloperFileType.DesignerFile:
                    {
                        return new DesignerFileCheckItemList(languageType);
                    }
                case DeveloperFileType.SourceFile:
                    {
                        return new SourceFileCheckItemList(languageType);
                    }
                case DeveloperFileType.WebFile:
                    {
                        return new WebFileCheckItemList();
                    }
            }

            return null;
        }
    }
}
