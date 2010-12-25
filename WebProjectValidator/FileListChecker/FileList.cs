using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WebProjectValidator.EnumTypes;

namespace WebProjectValidator.FileListChecker
{
    class FileList:List<string>
    {
        private CheckItemList _Checker = null;
        private string _ProjectFolder = string.Empty;

        private FolderFilterList _FolderFilterList = new FolderFilterList();

        public FileList(string projectFolder, LanguageType languageType, DeveloperFileType developerType)
        {
            _ProjectFolder = projectFolder;

            _Checker = GetChecker(languageType, developerType);

            List<string> tempList = new List<string>();

            tempList.AddRange(Directory.GetFiles(_ProjectFolder, "*.*", SearchOption.AllDirectories));

            foreach (string fileName in tempList)
            {
                if (_Checker.IsValid(fileName) && !_FolderFilterList.IsFiltered(fileName))
                {
                    this.Add(fileName);
                }
            }
        }

        public CheckItemList GetChecker(LanguageType languageType, DeveloperFileType developerType)
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
