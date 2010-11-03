using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebProjectValidator.FileListChecker;

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
                DesignerFileItem designerFileItem = new DesignerFileItem();

                designerFileItem.Project = _Project;
                designerFileItem.Folder = _Folder.ToLower();
                designerFileItem.FileName = fileName.Replace(designerFileItem.Folder, "");

                string findName = fileName.Replace(".cs", ".designer.cs");

                if (findList.Contains(findName))
                {
                    designerFileItem.InValid = false;
                }

                resultList.Add(designerFileItem);
            }

            return resultList;
        }

        public void processUserControl(FileList fileList)
        {
        }
    }
}
