using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebProjectValidator.FileListChecker
{
    class FileTypeChecker
    {
        private LanguageFileType _LanguageFileType = LanguageFileType.cs;
        private CheckItemList checkItemList = null;

        #region simple properties
        internal LanguageFileType FileType
        {
            get { return _LanguageFileType; }
            set { _LanguageFileType = value; }
        }
        #endregion

        public FileTypeChecker(LanguageFileType languageType)
        {
            _LanguageFileType = languageType;
        }

        public void SetFileType(DeveloperFileType developerType)
        {
            switch (developerType)
            {
                case DeveloperFileType.DesignerFile:
                    {
                        //checkItemList.InitCs(_LanguageFileType);
                        break;
                    }
                case DeveloperFileType.SourceFile:
                    {
                        //checkItemList.InitVb(_LanguageFileType);
                        break;
                    }
                case DeveloperFileType.WebFile:
                    {
                        checkItemList = new WebCheckItemList();
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
