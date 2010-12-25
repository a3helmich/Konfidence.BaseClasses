using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebProjectValidator.EnumTypes;

namespace WebProjectValidator.FileListChecker
{
    class DesignerFileCheckItemList: CheckItemList
    {
        public DesignerFileCheckItemList(LanguageType languageType)
        {
            ListFiller.FillDesignerFileCheckItemList(languageType, this);
        }
    }
}
