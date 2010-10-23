using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebProjectValidator.HelperClasses
{
    class CheckItem
    {
        private string _TextFragment = string.Empty;
        private CheckAction _Action = CheckAction.Unknown;

        #region properties
        public string TextFragment
        {
            get { return _TextFragment; }
            set { _TextFragment = value; }
        }

        public CheckAction Action
        {
            get { return _Action; }
            set { _Action = value; }
        }
        #endregion properties

        public CheckItem(string namePart, CheckAction action)
        {
            // TODO: Complete member initialization
            _TextFragment = namePart;
            _Action = action;
        }

        public bool IsValid(string text)
        {
            switch (_Action)
            {
                case CheckAction.Contains:
                    {
                        if (text.Contains(_TextFragment))
                        {
                            return true;
                        }
                        break;
                    }
                case CheckAction.EndsWith:
                    {
                        if (text.EndsWith(_TextFragment))
                        {
                            return true;
                        }
                        break;
                    }
            }

            return false;
        }
    }
}
