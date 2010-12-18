using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;
using System.IO;

namespace WebProjectValidator
{
    class MainFormPresenter: BaseItem
    {
        private string _SolutionFolder = string.Empty;
        private string _ProjectName = string.Empty;

        #region simple properties
        public string SolutionFolder
        {
            get 
            { 
                return _SolutionFolder; 
            }
            set 
            {
                _SolutionFolder = value;

                if (!_SolutionFolder.EndsWith(@"\"))
                {
                    _SolutionFolder += @"\";
                }
            }
        }

        public string ProjectFolder
        {
            get
            {
                return SolutionFolder + ProjectName;
            }
        }

        public string ProjectName
        {
            get
            {
                return _ProjectName;
            }

            set
            {
                _ProjectName = value;
            }
        }
#endregion simple properties

        public bool Validate()
        {
            ClearErrorMessage();

            if (IsEmpty(_SolutionFolder))
            {
                SetErrorMessage("Selecteer een solution folder!");
            }
            else
            {
                if (!Directory.Exists(ProjectFolder))
                {
                    SetErrorMessage("Folder: '" + ProjectFolder + "' bestaat niet!");
                }
            }

            return !HasErrors();
        }
    }
}
