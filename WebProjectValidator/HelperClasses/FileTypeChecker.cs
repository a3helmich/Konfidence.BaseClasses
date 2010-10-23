using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebProjectValidator.HelperClasses
{
    class FileTypeChecker
    {
        private FileType _FileType = FileType.cs;

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
    }
}
