using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebProjectValidator.FileListChecker
{
    class ListFiller
    {
        public static void FillFolderFilterList(FolderFilterList folderFilterList)
        {
            folderFilterList.Clear();

            folderFilterList.Add(@"\fileBackup\");
            folderFilterList.Add(@"\bin\debug\");
            folderFilterList.Add(@"\bin\release\");
            folderFilterList.Add(@"\App_Data\");
        }

        internal static void FillWebCheckItemList(WebCheckItemList webCheckItemList)
        {
            webCheckItemList.Clear();

            webCheckItemList.AddItem(".aspx", CheckAction.EndsWith);
            webCheckItemList.AddItem(".ascx", CheckAction.EndsWith);
            webCheckItemList.AddItem(".master", CheckAction.EndsWith);
        }
    }
}
