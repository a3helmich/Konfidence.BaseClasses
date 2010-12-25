using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebProjectValidator.EnumTypes;

namespace WebProjectValidator.FileListChecker
{
    class CheckItemList:List<CheckItem>
    {
        public CheckItemList()
        {
        }

        public void AddItem(string namePart, CheckAction action)
        {
            CheckItem checkItem = new CheckItem(namePart, action);

            Add(checkItem);
        }

        public bool IsValid(string fileName)
        {
            foreach (CheckItem checkItem in this)
            {
                if (checkItem.IsValid(fileName))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
