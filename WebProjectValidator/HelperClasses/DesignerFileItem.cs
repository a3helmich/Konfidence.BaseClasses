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
        private bool _Exists = false;
        private bool _IsUsed = true;

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

        public bool Exists
        {
            get { return _Exists; }
            set { _Exists = value; }
        }

        public bool IsUsed
        {
            get { return _IsUsed; }
            set { _IsUsed = value; }
        }

        public string FullFileName
        {
            get { return _ProjectFolder + @"\" + _FileName; }
        }
#endregion simple properties

        public DesignerFileItem(string project, string projectFolder, string fileName)
        {
            _Project = project;
            _ProjectFolder = projectFolder;

            _FileName = fileName.Replace(ProjectFolder, string.Empty);

            int deviderIndex = FileName.LastIndexOf(@"\");
            if (deviderIndex > 0)
            {
                _ControlFolder = FileName.Substring(0, deviderIndex);
                _FileName = FileName.Substring(deviderIndex);
            }

            _FileName = FileName.Replace(@"\", string.Empty);
        }
    }
}
