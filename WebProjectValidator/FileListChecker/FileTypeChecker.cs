using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebProjectValidator.FileListChecker
{
    class FileTypeChecker
    {
        private FileType _FileType = FileType.cs;
        private CheckItemList checkItemList = new CheckItemList();

        #region simple properties
        internal FileType FileType
        {
            get { return _FileType; }
            set { _FileType = value; }
        }
        #endregion

        public FileTypeChecker()
        {

        }

        public void SetFileType(FileType type)
        {
            switch (type)
            {
                case FileType.cs:
                    {
                        checkItemList.InitCs();
                        break;
                    }
                case FileType.vb:
                    {
                        checkItemList.InitVb();
                        break;
                    }
                case FileType.web:
                    {
                        checkItemList.InitWeb();
                        break;
                    }
            }
        }

        public bool IsValid(string fileName)
        {
            return checkItemList.IsValid(fileName);
        }
    }
}
