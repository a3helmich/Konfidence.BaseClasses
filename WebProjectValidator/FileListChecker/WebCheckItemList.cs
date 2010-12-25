using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebProjectValidator.FileListChecker
{
    class WebCheckItemList:CheckItemList
    {
        public WebCheckItemList()
        {
            ListFiller.FillWebCheckItemList(this);
        }
    }
}
