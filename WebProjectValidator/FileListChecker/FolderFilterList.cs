using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebProjectValidator.FileListChecker
{
    class FolderFilterList:List<string>
    {
        public FolderFilterList()
        {
            ListFiller.FillFolderFilterList(this);
        }

        public bool IsFiltered(string fileName)
        {
            foreach (string folder in this)
            {
                if (fileName.IndexOf(folder, StringComparison.InvariantCultureIgnoreCase) > -1)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
