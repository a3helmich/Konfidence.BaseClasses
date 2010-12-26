﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebProjectValidator.FileListChecker;

namespace WebProjectValidator.HelperClasses
{
    public class DesignerFileItemList : List<DesignerFileItem>
    {
        private FileList fileList;

        public DesignerFileItemList(string projectFolder, FileList fileList)
        {
            foreach (string fileName in fileList)
            {
                DesignerFileItem designerFileItem = new DesignerFileItem(projectFolder, fileName);
            }
        }
    }
}
