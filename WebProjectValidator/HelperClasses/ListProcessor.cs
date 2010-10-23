using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebProjectValidator.HelperClasses
{
    class ListProcessor
    {
        public static void processCodeFileCheck(FileList fileList)
        {
        }

        public static List<string> processDesignerFile(FileList fileList)
        {
            List<string> findList = new List<string>(fileList);
            List<string> resultList = new List<string>();

            foreach (string fileName in fileList)
            {
                string findName = fileName.Replace(".cs", "designer.cs");

                if (!findList.Contains(findName))
                {
                    resultList.Add(findName);
                }
            }

            return resultList;
        }

        public static void processUserControl(FileList fileList)
        {
        }
    }
}
