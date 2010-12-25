using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebProjectValidator.FileListChecker
{
    class WebFileCheckItemList:CheckItemList
    {
        public WebFileCheckItemList()
        {
            ListFiller.FillWebCheckItemList(this);
        }
    }
}
