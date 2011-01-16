using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebProjectValidator.FileListChecker;

namespace WebProjectValidator.HelperClasses
{
    public class ApplicationFileItemList : List<ApplicationFileItem>
    {
        public ApplicationFileItemList(string projectFolder, FileList fileList)
        {
            foreach (string fileName in fileList)
            {
                ApplicationFileItem designerFileItem = new ApplicationFileItem(projectFolder, fileName);

                this.Add(designerFileItem);
            }
        }
    }
}
