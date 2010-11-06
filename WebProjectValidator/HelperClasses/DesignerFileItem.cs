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
        private string _ProjectFolder = string.Empty;
        private string _ControlFolder = ".";
        private string _FileName = string.Empty;
        private bool _Valid = false;

        #region simple properties
        public string Project
        {
            get { return _Project; }
            set { _Project = value; }
        }

        public string ProjectFolder
        {
            get { return _ProjectFolder; }
            set { _ProjectFolder = value; }
        }

        public string ControlFolder
        {
            get { return _ControlFolder; }
            set { _ControlFolder = value; }
        }

        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        public bool Valid
        {
            get { return _Valid; }
            set { _Valid = value; }
        }
#endregion simple properties

    }
}
