using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebProjectValidator.FileListChecker
{
    enum ListType
    {
        Excluded = 0,
        Included = 1
    }

    class CheckItemList:List<CheckItem>
    {
        private ListType _Type = ListType.Included;

        public CheckItemList(ListType type)
        {
            _Type = type;

            InitCs();
        }

        public CheckItem AddItem(string namePart, CheckAction action)
        {
            CheckItem checkItem = new CheckItem(namePart, action);

            Add(checkItem);

            return checkItem;
        }

        public void InitCs()
        {
            Clear();

            switch (_Type)
            {
                case ListType.Included:
                    AddItem(".aspx.cs", CheckAction.EndsWith);
                    AddItem(".ascx.cs", CheckAction.EndsWith);
                    AddItem(".master.cs", CheckAction.EndsWith);
                    break;
                case ListType.Excluded:
                    AddItem(".designer.cs", CheckAction.EndsWith);
                    break;
            }
        }

        public void InitVb()
        {
            Clear();

            switch (_Type)
            {
                case ListType.Included:
                    AddItem(".aspx.vb", CheckAction.EndsWith);
                    AddItem(".ascx.vb", CheckAction.EndsWith);
                    AddItem(".master.vb", CheckAction.EndsWith);
                    break;
                case ListType.Excluded:
                    AddItem(".designer.vb", CheckAction.EndsWith);
                    break;
            }
        }

        internal void InitWeb()
        {
            Clear();

            AddItem(".aspx", CheckAction.EndsWith);
            AddItem(".ascx", CheckAction.EndsWith);
            AddItem(".master", CheckAction.EndsWith);
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
