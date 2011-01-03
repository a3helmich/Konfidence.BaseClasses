using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebProjectValidator.EnumTypes;

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

        internal static void FillWebCheckItemList(WebFileCheckItemList webCheckItemList)
        {
            webCheckItemList.Clear();

            webCheckItemList.AddItem(".aspx", CheckAction.EndsWith);
            webCheckItemList.AddItem(".ascx", CheckAction.EndsWith);
            webCheckItemList.AddItem(".master", CheckAction.EndsWith);
        }

        internal static void FillSourceFileCheckItemList(LanguageType languageType, SourceFileCheckItemList developerFileCheckItemList)
        {
            developerFileCheckItemList.Clear();

            string extension = GetExtension(languageType);

            developerFileCheckItemList.AddItem(".aspx" + extension, CheckAction.EndsWith);
            developerFileCheckItemList.AddItem(".ascx" + extension, CheckAction.EndsWith);
            developerFileCheckItemList.AddItem(".master" + extension, CheckAction.EndsWith);
        }

        internal static void FillDesignerFileCheckItemList(LanguageType languageType, DesignerFileCheckItemList designerFileCheckItemList)
        {
            designerFileCheckItemList.Clear();

            string extension = GetExtension(languageType);

            designerFileCheckItemList.AddItem(".designer" + extension, CheckAction.EndsWith);
        }

        private static string GetExtension(LanguageType languageType)
        {
            switch (languageType)
            {
                case LanguageType.cs:
                    {
                        return ".cs";
                    }
                case LanguageType.vb:
                    {
                        return ".vb";
                    }
            }

            return string.Empty;
        }
    }
}
