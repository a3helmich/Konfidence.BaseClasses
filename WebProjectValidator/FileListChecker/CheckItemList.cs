using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebProjectValidator.FileListChecker
{
    class CheckItemList:List<CheckItem>
    {
        private DeveloperFileType _Type = DeveloperFileType.SourceFile;

        public CheckItemList()
        {
        }

        public CheckItemList(DeveloperFileType type)
        {
            _Type = type;

            InitCs();
        }

        public void AddItem(string namePart, CheckAction action)
        {
            CheckItem checkItem = new CheckItem(namePart, action);

            Add(checkItem);
        }

        public void InitCs()
        {
            Clear();

            switch (_Type)
            {
                case DeveloperFileType.SourceFile:
                    AddItem(".aspx.cs", CheckAction.EndsWith);
                    AddItem(".ascx.cs", CheckAction.EndsWith);
                    AddItem(".master.cs", CheckAction.EndsWith);
                    break;
                case DeveloperFileType.DesignerFile:
                    AddItem(".designer.cs", CheckAction.EndsWith);
                    break;
            }
        }

        public void InitVb()
        {
            Clear();

            switch (_Type)
            {
                case DeveloperFileType.SourceFile:
                    AddItem(".aspx.vb", CheckAction.EndsWith);
                    AddItem(".ascx.vb", CheckAction.EndsWith);
                    AddItem(".master.vb", CheckAction.EndsWith);
                    break;
                case DeveloperFileType.DesignerFile:
                    AddItem(".designer.vb", CheckAction.EndsWith);
                    break;
            }
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
