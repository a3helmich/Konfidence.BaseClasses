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

        public bool IsFiltered(string fileName)
        {
            if (fileName.IndexOf(@"\fileBackup\", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return true;
            }

            if (fileName.IndexOf(@"\bin\debug\", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return true;
            }

            if (fileName.IndexOf(@"\bin\release\", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return true;
            }

            return false;
        }

        public FileList(string projectFolder, FileType fileType, ListType listType)
        {
            _ProjectFolder = projectFolder;

            _Checker = new FileTypeChecker(listType);

            _Checker.SetFileType(fileType);

            List<string> tempList = new List<string>();

            tempList.AddRange(Directory.GetFiles(_ProjectFolder, "*.*", SearchOption.AllDirectories));

            foreach (string fileName in tempList)
            {
                if (_Checker.IsValid(fileName) && !IsFiltered(fileName))
                {
                    this.Add(fileName);
                }
            }
        }

    }
}
