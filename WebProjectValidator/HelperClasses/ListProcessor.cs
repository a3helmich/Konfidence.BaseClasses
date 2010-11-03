using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebProjectValidator.HelperClasses
{
    class ListProcessor
    {
        private string _Project = string.Empty;
        private string _Folder = string.Empty;

        public ListProcessor(string project, string folder)
        {
            _Project = project;
            _Folder = folder;
        }

        public void processCodeFileCheck(FileList fileList)
        {
            
        }

        public List<DesignerFileItem> processDesignerFile(FileList fileList)
        {
            List<string> findList = new List<string>(fileList);
            List<DesignerFileItem> resultList = new List<DesignerFileItem>();

            foreach (string fileName in fileList)
            {
                string findName = fileName.Replace(".cs", ".designer.cs");

                if (!findList.Contains(findName))
                {
                    resultList.Add(findName);
                }
            }

            return resultList;
        }

        public void processUserControl(FileList fileList)
        {
        }
    }
}
