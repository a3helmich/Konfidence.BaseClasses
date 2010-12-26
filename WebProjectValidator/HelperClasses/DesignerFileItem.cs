using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;

namespace WebProjectValidator.HelperClasses
{
    public class DesignerFileItem: BaseItem
    {
        private string _ProjectFolder = string.Empty;
        private string _ControlFolder = ".";
        private string _FileName = string.Empty;

        private ControlReference _ControlReference = null;

        private bool _Valid = false;
        private bool _Exists = false;
        private bool _IsUsed = true;

        #region simple properties

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
#endregion simple properties

        public string Reference
        {
            get
            {
                if (IsAssigned(_ControlReference))
                {
                    return _ControlReference.Reference;
                }

                return string.Empty;
            }
        }

        public string FullFileName
        {
            get 
            {
                string fullFileName = string.Empty;

                if (_FileName.StartsWith("~"))
                {
                    fullFileName = _FileName.Replace("~", _ProjectFolder);
                }
                else
                {
                    if (_ControlFolder == ".")
                    {
                        fullFileName = _ProjectFolder + @"\" + _FileName;
                    }
                    else
                    {
                        if (_ControlFolder.StartsWith("~"))
                        {
                            fullFileName = _ControlFolder.Replace("~", _ProjectFolder) + @"\" + _FileName;
                        }
                        else
                        {
                            fullFileName = _ProjectFolder + @"\" + _ControlFolder + @"\" + _FileName;
                        }
                    }
                }

                fullFileName = fullFileName.Replace("/", @"\");

                return fullFileName;
            }
        }

        public DesignerFileItem(string projectFolder, ControlReference controlReference)
            : this(projectFolder, controlReference.FileName)
        {
            _ControlReference = controlReference;
        }
        
        public DesignerFileItem(string projectFolder, string fileName)
        {
            _ProjectFolder = projectFolder;

            _FileName = this.ReplaceIgnoreCase(fileName, ProjectFolder, string.Empty);

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
