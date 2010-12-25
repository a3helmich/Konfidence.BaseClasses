using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebProjectValidator.EnumTypes;

namespace WebProjectValidator.FileListChecker
{
    class SourceFileCheckItemList : CheckItemList
    {
        public SourceFileCheckItemList(LanguageType languageType)
        {
            ListFiller.FillSourceFileCheckItemList(languageType, this);
        }
    }
}
