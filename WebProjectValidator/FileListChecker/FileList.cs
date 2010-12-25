using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WebProjectValidator.FileListChecker
{
    class FileList:List<string>
    {
        private FileTypeChecker _Checker = null;
        private string _ProjectFolder = string.Empty;

        private FolderFilterList _FolderFilterList = new FolderFilterList();

        public FileList(string projectFolder, LanguageFileType fileType, DeveloperFileType listType)
        {
            _ProjectFolder = projectFolder;

            _Checker = new FileTypeChecker(fileType);

            _Checker.SetFileType(listType);

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

    }
}
