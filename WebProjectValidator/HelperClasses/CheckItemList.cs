using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebProjectValidator.HelperClasses
{
    class CheckItemList:List<CheckItem>
    {
        public CheckItem AddItem(string namePart, CheckAction action)
        {
            CheckItem checkItem = new CheckItem(namePart, action);

            return checkItem;
        }

        public void InitCs()
        {
            Clear();

            AddItem(".aspx.cs", CheckAction.EndsWith);
            AddItem(".ascx.cs", CheckAction.EndsWith);
        }

        public void InitVb()
        {
            Clear();

            AddItem(".aspx.vb", CheckAction.EndsWith);
            AddItem(".ascx.vb", CheckAction.EndsWith);
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
