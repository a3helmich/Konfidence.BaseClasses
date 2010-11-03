using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;

namespace WebProjectValidator.HelperClasses
{
    public class DesignerFileItem: BaseItem
    {
        private string _Project = string.Empty;
        private string _Folder = string.Empty;
        private string _FileName = string.Empty;
        private bool _InValid = true;

        public string Project
        {
            get { return _Project; }
            set { _Project = value; }
        }

        public string Folder
        {
            get { return _Folder; }
            set { _Folder = value; }
        }

        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        public bool InValid
        {
            get { return _InValid; }
            set { _InValid = value; }
        }
    }
}
