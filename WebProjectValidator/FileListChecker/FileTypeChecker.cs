using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebProjectValidator.EnumTypes;

namespace WebProjectValidator.FileListChecker
{
    class FileTypeChecker
    {
        private LanguageType _LanguageType = LanguageType.cs;
        private CheckItemList checkItemList = null;

        #region simple properties
        internal LanguageType FileType
        {
            get { return _LanguageType; }
            set { _LanguageType = value; }
        }
        #endregion

        public FileTypeChecker(LanguageType languageType)
        {
            _LanguageType = languageType;
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
